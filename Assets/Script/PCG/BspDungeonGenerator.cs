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

    /*[Header("Corridors")]
    [SerializeField] private int _corridorSize = 3;*/
    
    [Header("Walls")]
    [SerializeField] private Tilemap _tilemapWalls;
    [SerializeField] private RuleTile _tileWall;

    [Header("Player")]
    [SerializeField] private GameObject _playerPrefab;
    private GameObject _spawnedPlayer;
    
    [Header("Enemies")]
    [SerializeField] private EnemySpawner _enemySpawner;

    [Header("Markov / Room Types")]
    [SerializeField] private RoomStereotype _startRoomStereotype;
    private List<RoomStereotype> _roomSequence = new List<RoomStereotype>();
    [SerializeField] private RoomStereotype _smallFightRoomStereotype; // fallback
    [SerializeField] private RoomStereotype _bossRoomStereotype;

    [Header("Room Visuals")]
    [SerializeField] private GameObject _bossRoomPrefab;        // red
    [SerializeField] private GameObject _smallFightRoomPrefab;  // yellow
    [SerializeField] private GameObject _shopRoomPrefab;        // blue
    [SerializeField] private GameObject _bigFightRoomPrefab;    // purple
    private List<GameObject> _spawnedRoomVisuals = new List<GameObject>();

    private List<BoundsInt> _rooms = new List<BoundsInt>();
    private List<BoundsInt> _cutBounds = new List<BoundsInt>();
    private List<Corridor> _corridors = new List<Corridor>();
    private List<Vector2Int> _corridorTiles = new List<Vector2Int>();
    private BoundsInt _spawnRoom;

    void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        Vector2Int corner = _startPos - _size / 2;
        BoundsInt originalRoom = new BoundsInt(
            new Vector3Int(corner.x, corner.y, 1),
            new Vector3Int(_size.x, _size.y, 1)
        );

        PCG.BspTree.Generate(out BspNode root, originalRoom, _minCutRatio, _maxCutRatio, _maxSurface, _jitter, _shrinkFactor);
        _rooms = PCG.BspTree.GetRooms(root, _shrinkFactor);
        _cutBounds = PCG.BspTree.GetBounds(root);
        _corridors = PCG.BspTree.MakeCorridors(root);

        _corridorTiles.Clear();
        foreach (Corridor corridor in _corridors)
            _corridorTiles.AddRange(PCG.Corridor.LCorridors(corridor, Corridor.CorridorIntersectType.FirstXThenY, 2));

        _tilemapWalls?.ClearAllTiles();
        _tilemapGround.ClearAllTiles();
        _tilemapCorridors.ClearAllTiles();
        _enemySpawner?.ClearEnemies();
        foreach (var obj in _spawnedRoomVisuals)
            if (obj != null) DestroyImmediate(obj);
        _spawnedRoomVisuals.Clear();

        if (_spawnedPlayer != null)
        {
            DestroyImmediate(_spawnedPlayer);
            _spawnedPlayer = null;
        }
        foreach (BoundsInt room in _rooms)
            PCG.Utils.DrawMap(_tilemapGround, _tileGround, room);
        PCG.Utils.DrawMap(_tilemapCorridors, _tileCorridor, _corridorTiles);

        GenerateWalls();
        GenerateRoomSequence(_rooms.Count);
        SpawnPlayer();
        SpawnRoomVisuals();
        _enemySpawner?.SpawnEnemies(_rooms, _roomSequence);
    }

    // ── Markov ────────────────────────────────────────────────
    private void GenerateRoomSequence(int count)
    {
        _roomSequence.Clear();
        RoomStereotype current = _startRoomStereotype;

        for (int i = 0; i < count; i++)
        {
            if (current == null) break;
            _roomSequence.Add(current);
            current = current.NextRoom();
        }
        
        EnforceBossRule();

        string seq = string.Join(" → ", _roomSequence.Select(r => r?.Type.ToString() ?? "null"));
        Debug.Log($"[Markov] Séquence : {seq}");
        
        
    }
    
    private void EnforceBossRule()
    {
        // Trouver tous les index Boss générés
        var bossIndexes = _roomSequence
            .Select((r, i) => (r, i))
            .Where(x => x.r != null && x.r.Type == RoomType.Boss)
            .Select(x => x.i)
            .ToList();

        if (bossIndexes.Count == 0)
        {
            // Aucun boss → forcer le dernier slot en Boss
            _roomSequence[_roomSequence.Count - 1] = _bossRoomStereotype;
            Debug.Log("[Markov] Aucun Boss trouvé → forcé en dernière room");
        }
        else if (bossIndexes.Count > 1)
        {
            // Trop de boss → garder uniquement le dernier, remplacer les autres par Fight
            int keepIndex = bossIndexes.Last();
            foreach (int i in bossIndexes.Where(i => i != keepIndex))
            {
                _roomSequence[i] = _smallFightRoomStereotype;
                Debug.Log($"[Markov] Boss en double supprimé à l'index {i}");
            }
        }
    }

    // ── Spawn Player ──────────────────────────────────────────
    private void SpawnPlayer()
    {
        if (_playerPrefab == null || _rooms == null || _rooms.Count == 0) return;

        if (_spawnedPlayer != null) Destroy(_spawnedPlayer);

        _spawnRoom = _rooms[0];

        Vector3 spawnPos = _spawnRoom.center;
        spawnPos.z = 0;

        _spawnedPlayer = Instantiate(_playerPrefab, spawnPos, Quaternion.identity);
        _spawnedPlayer.name = "Player";

        Debug.Log($"[Spawn] Player → room 0 ({_roomSequence[0]?.Type}) at {spawnPos}");
    }

    // ── Room Visuals ──────────────────────────────────────────
    private void SpawnRoomVisuals()
    {
        foreach (var obj in _spawnedRoomVisuals)
            if (obj != null) Destroy(obj);
        _spawnedRoomVisuals.Clear();

        for (int i = 0; i < _rooms.Count; i++)
        {
            if (i >= _roomSequence.Count || _roomSequence[i] == null) continue;

            GameObject prefab = GetPrefabForRoomType(_roomSequence[i].Type);
            if (prefab == null) continue;

            Vector3 pos = _rooms[i].center;
            pos.z = 0;

            GameObject visual = Instantiate(prefab, pos, Quaternion.identity);
            visual.name = $"RoomVisual_{_roomSequence[i].Type}_{i}";
            _spawnedRoomVisuals.Add(visual);
        }
    }

    private GameObject GetPrefabForRoomType(RoomType type)
    {
        return type switch
        {
            RoomType.Boss  => _bossRoomPrefab,
            RoomType.Fight => _smallFightRoomPrefab,
            RoomType.BigFight => _bigFightRoomPrefab,
            RoomType.Shop  => _shopRoomPrefab,
            _              => null
        };
    }
    
    private void GenerateWalls()
    {
        if (_tilemapWalls == null || _tileWall == null) return;

        _tilemapWalls.ClearAllTiles();

        // 1. Collecter toutes les positions de sol
        HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();

        foreach (BoundsInt room in _rooms)
        foreach (var pos in room.allPositionsWithin)
            floorTiles.Add(new Vector2Int(pos.x, pos.y));

        foreach (var pos in _corridorTiles)
            floorTiles.Add(pos);

        // 2. Pour chaque tile de sol, vérifier les 8 voisins
        HashSet<Vector2Int> wallTiles = new HashSet<Vector2Int>();

        foreach (Vector2Int tile in floorTiles)
        {
            foreach (Vector2Int dir in PCG.Utils.MooreDirections)
            {
                Vector2Int neighbor = tile + dir;
                if (!floorTiles.Contains(neighbor))
                    wallTiles.Add(neighbor);
            }
        }
        
        foreach (Vector2Int wall in wallTiles)
            _tilemapWalls.SetTile(new Vector3Int(wall.x, wall.y, 0), _tileWall);
    }

    // ── Gizmos ────────────────────────────────────────────────
    private void OnDrawGizmos()
    {
        foreach (BoundsInt cutBound in _cutBounds)
        {
            Gizmos.color = Color.white;
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
        }

        // SpawnRoom
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_spawnRoom.center, _spawnRoom.size);
    }
}