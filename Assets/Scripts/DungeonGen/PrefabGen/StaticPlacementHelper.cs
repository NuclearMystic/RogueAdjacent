using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticPlacementHelper
{
    private readonly Dictionary<PlacementType, HashSet<Vector2Int>> tileByType =
        new Dictionary<PlacementType, HashSet<Vector2Int>>();

    private readonly HashSet<Vector2Int> roomFloorNoCorridor;

    public StaticPlacementHelper(HashSet<Vector2Int> roomFloor,
                                 HashSet<Vector2Int> roomFloorNoCorridor)
    {
        this.roomFloorNoCorridor = roomFloorNoCorridor;

        // Initialize every key to avoid KeyNotFoundException
        foreach (PlacementType t in Enum.GetValues(typeof(PlacementType)))
            tileByType[t] = new HashSet<Vector2Int>();

        var graph = new Graph(roomFloor);

        foreach (var position in roomFloorNoCorridor)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            var type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;

            // Skip inner walls-only tiles for NearWall (like you had before)
            if (type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count == 4)
                continue;

            tileByType[type].Add(position);
        }
    }

    public Vector2? GetItemPlacementPosition(PlacementType requestedType,
                                             int iterationsMax,
                                             Vector2Int size,
                                             bool addOffset)
    {
        // Pick a bucket (with fallback)
        var bucket = GetBestBucket(requestedType, size);
        if (bucket == null || bucket.Count == 0)
            return null;

        int iteration = 0;
        int itemArea = size.x * size.y;

        while (iteration < iterationsMax && bucket.Count >= itemArea)
        {
            iteration++;

            // Random pick
            int index = UnityEngine.Random.Range(0, bucket.Count);
            Vector2Int position = bucket.ElementAt(index);

            if (itemArea > 1)
            {
                var (ok, placementPositions) = PlaceBigItem(position, size, addOffset);
                if (!ok)
                    continue;

                // Remove the occupied positions from all buckets we care about
                RemoveFromBucket(requestedType, placementPositions);
                RemoveFromBucket(PlacementType.NearWall, placementPositions);
                RemoveFromBucket(PlacementType.OpenSpace, placementPositions);
            }
            else
            {
                bucket.Remove(position);
                // Also remove from the other bucket (if present) to avoid double-use
                RemoveFromBucket(PlacementType.NearWall, position);
                RemoveFromBucket(PlacementType.OpenSpace, position);
            }

            return position;
        }

        return null;
    }

    private HashSet<Vector2Int> GetBestBucket(PlacementType requestedType, Vector2Int size)
    {
        int area = size.x * size.y;

        // Primary
        if (tileByType.TryGetValue(requestedType, out var primary) && primary.Count >= area)
            return primary;

        // Fallback to the other type
        var other = requestedType == PlacementType.OpenSpace ? PlacementType.NearWall : PlacementType.OpenSpace;
        if (tileByType.TryGetValue(other, out var secondary) && secondary.Count >= area)
        {
            Debug.LogWarning($"StaticPlacementHelper: No tiles for {requestedType}, using {other} instead.");
            return secondary;
        }

        // Final fallback: try any tile that fits
        var union = new HashSet<Vector2Int>(
            tileByType.Values.SelectMany(v => v)
        );
        return union.Count >= area ? union : null;
    }

    private void RemoveFromBucket(PlacementType type, IEnumerable<Vector2Int> positions)
    {
        if (tileByType.TryGetValue(type, out var set))
            set.ExceptWith(positions);
    }

    private void RemoveFromBucket(PlacementType type, Vector2Int position)
    {
        if (tileByType.TryGetValue(type, out var set))
            set.Remove(position);
    }

    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition,
                                                  Vector2Int size,
                                                  bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int> { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if (col == 0 && row == 0) continue;

                Vector2Int newPosToCheck = new Vector2Int(originPosition.x + row, originPosition.y + col);
                if (!roomFloorNoCorridor.Contains(newPosToCheck))
                    return (false, positions);

                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    }
}

public enum PlacementType
{
    OpenSpace,
    NearWall
}
