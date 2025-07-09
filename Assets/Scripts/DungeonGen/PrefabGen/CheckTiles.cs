using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CheckTiles : MonoBehaviour
{
    public GameObject spawnPoint;

    private TilemapVisualizer tilemapVisualizer;
    private Tilemap floorTilemap;

    private List<TileBase> tiles = new List<TileBase>();

    private void Start()
    {
        tilemapVisualizer = FindAnyObjectByType<TilemapVisualizer>();
        floorTilemap = tilemapVisualizer.floorTilemap;
        tiles = TilemapHelper.GetAllTiles(floorTilemap);
        Debug.Log(tiles.Count);
        CheckForFloorTile(tiles);
    }

    private TileBase CheckForFloorTile(List<TileBase> tiles)
    {
        // check each tile in tiles
        foreach (TileBase tile in tiles)
        {
            if (tile.name == "cave_floor_1")
            {
                Debug.Log("Floor tile found! " + tile.name);
                //Instantiate(spawnPoint, spawnPosition, Quaternion.identity);
                return tile;
            }
        }

        Debug.Log("No floor tiles found.");
        return null;
    }

    // check each tile to match binary 00000000

    // chache as first spawnable tile

}
