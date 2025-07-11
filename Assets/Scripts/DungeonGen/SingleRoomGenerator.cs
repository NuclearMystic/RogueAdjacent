using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SingleRoomGenerator : MonoBehaviour
{
    public TilemapVisualizer tilemapVisualizer;
    public int roomWidth = 10;
    public int roomHeight = 10;

    private void Start()
    {
        GenerateRoom();
    }

    public void GenerateRoom()
    {
        HashSet<Vector2Int> floorPositions = GenerateFloorPositions();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private HashSet<Vector2Int> GenerateFloorPositions()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                floorPositions.Add(new Vector2Int(x, y));
            }
        }
        return floorPositions;
    }

    public void SaveRoomAsPrefab(string prefabName)
    {
        GameObject roomPrefab = new GameObject(prefabName);
        Tilemap floorTilemap = Instantiate(tilemapVisualizer.floorTilemap, roomPrefab.transform);
        Tilemap wallTilemap = Instantiate(tilemapVisualizer.wallTilemap, roomPrefab.transform);

        // Save the prefab
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(roomPrefab, $"Assets/Prefabs/NewPrefab.prefab");
        Destroy(roomPrefab);
#endif
    }
}
