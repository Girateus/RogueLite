using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public static class Utils
{
    public static readonly Vector2Int[] VonNeumannDirections = new[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
    };

    public static readonly Vector3Int[] MooreDirections = new[]
    {
            new Vector3Int(0, 1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(-1, 1, 0)
    };

    public static void DrawMap(Tilemap map, TileBase tile, List<Vector2Int> generatedPositions)
    {
        foreach (Vector2Int position in generatedPositions)
        {
            map.SetTile(new Vector3Int(position.x, position.y, 0), tile);
        }
    }
    
    public static void DrawMap(Tilemap map, TileBase tile, Vector3Int[] generatedPositions)
    {
        foreach (Vector3Int position in generatedPositions)
        {
            map.SetTile(position, tile);
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


