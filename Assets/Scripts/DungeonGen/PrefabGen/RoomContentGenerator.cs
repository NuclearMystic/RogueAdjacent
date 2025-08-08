using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField] private RoomGenerator playerRoom, defaultRoom;
    [SerializeField] private GraphTest graphTest;
    [SerializeField] private Transform itemParent;
    [SerializeField] private InventoryItem[] dungeonLootTable;

    public UnityEvent RegenerateDungeon;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        GameManager.Instance.gameObject.GetComponent<LootTable>().dungeonItemsDatabase = dungeonLootTable;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CleanupSpawned();
            RegenerateDungeon?.Invoke();
        }
    }

    public void GenerateRoomContent(DungeonData dungeonData)
    {
        CleanupSpawned();

        if (dungeonData.roomsDictionary == null || dungeonData.roomsDictionary.Count == 0)
        {
            Debug.LogWarning("No rooms to populate in dungeonData.");
            return;
        }

        SelectPlayerSpawnPoint(dungeonData);

        // Snapshot what’s left so we don't iterate a collection we edit
        var remainingRooms = dungeonData.roomsDictionary.ToList();

        foreach (var room in remainingRooms)
        {
            var spawned = defaultRoom.ProcessRoom(
                room.Key,
                room.Value,
                dungeonData.GetRoomFloorWithoutCorridors(room.Key)
            );
            spawnedObjects.AddRange(spawned);
        }

        ReparentSpawned(itemParent);
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int count = dungeonData.roomsDictionary.Count;
        if (count == 0)
        {
            Debug.LogError("roomsDictionary is empty – cannot pick a player spawn room.");
            return;
        }

        int randomRoomIndex = Random.Range(0, count);
        var kvp = dungeonData.roomsDictionary.ElementAt(randomRoomIndex);
        var playerSpawnPoint = kvp.Key;
        var roomTiles = kvp.Value;

        graphTest?.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        var placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            roomTiles,
            dungeonData.GetRoomFloorWithoutCorridors(playerSpawnPoint)
        );

        spawnedObjects.AddRange(placedPrefabs);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void CleanupSpawned()
    {
        // Prefer Destroy at runtime; DestroyImmediate is for editor-time ops.
        foreach (var go in spawnedObjects)
        {
            if (go != null) Destroy(go);
        }
        spawnedObjects.Clear();
    }

    private void ReparentSpawned(Transform parent)
    {
        if (parent == null) return;

        foreach (var go in spawnedObjects)
        {
            if (go != null)
                go.transform.SetParent(parent, false);
        }
    }
}
