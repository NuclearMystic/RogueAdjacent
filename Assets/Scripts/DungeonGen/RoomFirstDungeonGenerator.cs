using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField][Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;

    // PCG Data (what RoomContentGenerator expects)
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary =
        new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> corridorPositions;

    // Fire this so RoomContentGenerator can place the player/enemies/etc.
    public UnityEvent<DungeonData> OnDungeonFloorReady;

    protected override void RunProceduralGeneration()
    {
        GenerateBspDungeon();

        var data = new DungeonData
        {
            roomsDictionary = roomsDictionary,
            corridorPositions = corridorPositions ?? new HashSet<Vector2Int>(),
            floorPositions = floorPositions ?? new HashSet<Vector2Int>()
        };

        OnDungeonFloorReady?.Invoke(data);
    }

    private void GenerateBspDungeon()
    {
        roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
        floorPositions = new HashSet<Vector2Int>();
        corridorPositions = new HashSet<Vector2Int>();

        // 1) Split the space into rooms
        var bounds = new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0));
        List<BoundsInt> roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            bounds, minRoomWidth, minRoomHeight);

        if (roomsList == null || roomsList.Count == 0)
        {
            Debug.LogWarning("RoomFirstDungeonGenerator: BSP returned no rooms.");
            tilemapVisualizer.Clear();
            return;
        }

        // 2) Fill rooms either via random-walk (cavey) or simple rectangles (structured)
        HashSet<Vector2Int> roomFloors = randomWalkRooms
            ? CreateRoomsRandomly(roomsList)
            : CreateSimpleRooms(roomsList);

        floorPositions = new HashSet<Vector2Int>(roomFloors);

        // 3) Connect room centers with corridors
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));

        corridorPositions = ConnectRooms(roomCenters);
        floorPositions.UnionWith(corridorPositions);

        // 4) Build the dictionary the content system relies on
        roomsDictionary = BuildRoomsDictionary(roomsList, roomFloors);

        // 5) Paint to tilemap
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private Dictionary<Vector2Int, HashSet<Vector2Int>> BuildRoomsDictionary(
        List<BoundsInt> roomsList,
        HashSet<Vector2Int> roomFloors)
    {
        var dict = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

        foreach (var room in roomsList)
        {
            var tiles = new HashSet<Vector2Int>();
            // We use the inner area (offset) that we actually carved into floor
            for (int x = room.xMin + offset; x < room.xMax - offset; x++)
            {
                for (int y = room.yMin + offset; y < room.yMax - offset; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (roomFloors.Contains(pos))
                        tiles.Add(pos);
                }
            }

            if (tiles.Count == 0) continue;

            Vector2Int center = new Vector2Int(
                Mathf.RoundToInt(room.center.x),
                Mathf.RoundToInt(room.center.y));

            // In rare cases two rooms can share the same integer-rounded center. Ensure uniqueness.
            while (dict.ContainsKey(center))
                center += Vector2Int.right;

            dict.Add(center, tiles);
        }

        return dict;
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(
                Mathf.RoundToInt(roomBounds.center.x),
                Mathf.RoundToInt(roomBounds.center.y));

            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            foreach (var position in roomFloor)
            {
                // Constrain the random walk to stay inside the room bounds (respecting offset)
                if (position.x >= (roomBounds.xMin + offset) &&
                    position.x <= (roomBounds.xMax - offset) &&
                    position.y >= (roomBounds.yMin + offset) &&   // you had -offset here – likely a typo
                    position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

        if (roomCenters.Count == 0)
            return corridors;

        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            position += destination.y > position.y ? Vector2Int.up : Vector2Int.down;
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            position += destination.x > position.x ? Vector2Int.right : Vector2Int.left;
            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
}
