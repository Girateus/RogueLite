using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar_Pathfinder_Component : MonoBehaviour
{
    
    [SerializeField] private Tilemap _map;
    [SerializeField] private BoundsInt _boundsInt;
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    
    [Header("Debug")]
    [SerializeField] private Tilemap _pathMap;
    [SerializeField] private TileBase _pathTile;
    
    private List <Vector3Int> _walkables2 = new List<Vector3Int>();
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _map.CompressBounds();
        var mapBounds = _map.cellBounds;
        
        foreach (var pos in _map.cellBounds.allPositionsWithin )
        {
            if (_map.HasTile(pos)) _walkables2.Add(pos);
        }
        
        StartCoroutine(Pathfinding());
        
        Debug.Log("Fini");
    }

    IEnumerator Pathfinding()
    {
        var path = AStarProcess.Process(_walkables2, Vector3Int.FloorToInt(_start.position), Vector3Int.FloorToInt(_end.position));
        if (path.Length > 0)
        {
            _pathMap.ClearAllTiles();
            Utils.DrawMap(_pathMap, _pathTile, path);
        }
        yield return null;
    }
    
}
