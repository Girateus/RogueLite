using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCG
{
    public static class Utils
    {
        public static readonly Vector2Int[] VonNeumannDirections = new[]
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };

        public static readonly Vector2Int[] MooreDirections = new[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };

        public static void DrawMap(Tilemap map, TileBase tile, List<Vector2Int> generatedPositions)
        {
            foreach (Vector2Int position in generatedPositions)
            {
                map.SetTile(new Vector3Int(position.x, position.y, 0), tile);
            }
        }

        public static void DrawMap(Tilemap map, TileBase tile, BoundsInt positions)
        {
            foreach (var position in positions.allPositionsWithin)
            {
                map.SetTile(position, tile);
            }
        }
        
    }

}