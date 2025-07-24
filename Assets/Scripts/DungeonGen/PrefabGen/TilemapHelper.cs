using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapHelper
{
    public static List<TileBase> GetAllTiles(Tilemap tilemap)
    {
        List<TileBase> tiles = new List<TileBase>();

        // Get the bounds of the Tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Iterate through all positions within the Tilemap bounds
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            // Get the tile at the current position
            TileBase tile = tilemap.GetTile(pos);

            // If there is a tile at this position, add it to the list
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }
}
