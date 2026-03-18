using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;


using System;
using System.Collections.Generic;
using System.Linq;
using PCG;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{

	[SerializeField] private Vector2Int _size;
	[SerializeField] private Vector2Int _startPos;
	
	[SerializeField] private float _threshold = 0.5f;
	
	[SerializeField] private Tilemap _tilemapGround;
	[SerializeField] private TileBase _tileGround;
	
	[SerializeField] private Tilemap _tilemapCorridors;
	[SerializeField] private TileBase _tileCorridor;

	[Header("BSP")]
	[SerializeField][Range(0.1f, 1f)] private float _minCutRatio;
	[SerializeField][Range(0.1f, 1f)] private float _maxCutRatio;
	[SerializeField] private int _maxSurface = 1250;
	[SerializeField][Range(0.1f, 1f)] private float _shrinkFactor = 0.8f;
	[SerializeField] private float _jitter = 25;

	[Header("Corridors")]
	[SerializeField] private int _corridorSize = 3;
	
	[Header("Player")]
	[SerializeField] private GameObject _playerPrefab;
	private GameObject _spawnedPlayer;
	
	[Header("Enemies")]
	[SerializeField] private GameObject _playerEnemies;
	private GameObject _spawnedEnemies;
	
	private List<BoundsInt> _rooms = new List<BoundsInt>();
	private List<BoundsInt> _cutBounds = new List<BoundsInt>();
	private List<Corridor>	_corridors = new List<Corridor>();
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

	    // List<Vector2Int> generatedPositions = Generate();
	    // DrawMap(_tilemap, _tile, generatedPositions);

	    GenerateDungeon();

    }
    
    public void GenerateDungeon()
    {

	    Vector2Int corner = _startPos - _size / 2;
	    BoundsInt originalRoom = new BoundsInt(new Vector3Int(corner.x, corner.y, 1), new Vector3Int(_size.x, _size.y, 1));

	    PCG.BspTree.Generate(out BspNode root, originalRoom, _minCutRatio, _maxCutRatio, _maxSurface, _jitter, _shrinkFactor);
	    _rooms = PCG.BspTree.GetRooms(root, _shrinkFactor);
	    _cutBounds = PCG.BspTree.GetBounds(root);
	    _corridors = PCG.BspTree.MakeCorridors(root);

	    var corridorTiles = new List<Vector2Int>();
	    foreach (Corridor corridor in _corridors)
	    {
		    corridorTiles.AddRange(PCG.Corridor.LCorridors(corridor, Corridor.CorridorIntersectType.FirstXThenY));
	    }

	    _tilemapGround.ClearAllTiles();
	    _tilemapCorridors.ClearAllTiles();
	    foreach (BoundsInt room in _rooms)
	    {
		    Debug.Log($"Room : {room.min} , {room.size}");
		    PCG.Utils.DrawMap(_tilemapGround, _tileGround, room);
	    }
	    PCG.Utils.DrawMap(_tilemapCorridors, _tileCorridor, corridorTiles);
	    SpawnPlayer();
    }

    private void OnDrawGizmos()
    {
	    foreach (BoundsInt cutBound in _cutBounds)
	    { 
		    Gizmos.color = Color.bisque;
			Gizmos.DrawWireCube(cutBound.center, cutBound.size);
	    }

	    foreach (BoundsInt room in _rooms)
	    { 
		    Gizmos.color = Color.red;
		    Gizmos.DrawWireCube(room.center, room.size);
	    }
	    
	    foreach (Corridor corridor in _corridors)
	    {
		    Gizmos.color = Color.cyan;
		    Gizmos.DrawLine(corridor.A.Room.center, corridor.B.Room.center);
		    //
		    // Gizmos.color = Color.darkCyan;
		    // Gizmos.DrawLine(corridor.A.Bounds.center, corridor.B.Bounds.center);
		    
	    }
	    
    }

    private List<Vector2Int> Generate()
    {
	    List<Vector2Int> positions = new List<Vector2Int>();
	    
	    for(int x = 0; x < _size.x; x++)
	    {
		    for(int y =  0; y < _size.y; y++)
		    {
			    float perlinValue = Mathf.PerlinNoise(x / (float)_size.x, y / (float)_size.y);
			    if(perlinValue > _threshold)
			    {
				    positions.Add(new Vector2Int(_startPos.x + x, _startPos.y + y));
			    }
		    }
	    }
	    
	    return positions;
    }
    
    private void SpawnPlayer()
    {
	    if (_playerPrefab == null)
	    {
		    Debug.LogError("Player Prefab is not assigned in DungeonGenerator!");
		    return;
	    }

	    if (_rooms == null || _rooms.Count == 0)
	    {
		    Debug.LogError("Cannot spawn player: No rooms generated!");
		    return;
	    }
	    
	    if (_spawnedPlayer != null)
	    {
		    Destroy(_spawnedPlayer);
	    }
	    
	    int randomIndex = UnityEngine.Random.Range(0, _rooms.Count);
	    BoundsInt startRoom = _rooms[randomIndex];
	    
	    Vector3 spawnPosition = startRoom.center;
	    
	    spawnPosition.z = 0;
	    
	    _spawnedPlayer = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
	    
	    _spawnedPlayer.name = "Player";

	    Debug.Log($"Player spawned in room {randomIndex} at {spawnPosition}");
    }

}

