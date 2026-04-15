using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject _cyclopPrefab;
    [SerializeField] private GameObject _batPrefab;
    [SerializeField] private GameObject _greaterGhostPrefab;

    [Header("Spawn Count")]
    [SerializeField] private int _smallFightMin = 2;
    [SerializeField] private int _smallFightMax = 3;
    [SerializeField] private int _bigFightMin = 4;
    [SerializeField] private int _bigFightMax = 6;

    private MarkovStateEnemy _cyclop;
    private MarkovStateEnemy _bat;
    private MarkovStateEnemy _greaterGhost;

    private List<GameObject> _spawnedEnemies = new List<GameObject>();

    private void Awake()
    {
        
        _cyclop      = new MarkovStateEnemy("Cyclop");
        _bat = new MarkovStateEnemy("Bat");
        _greaterGhost = new MarkovStateEnemy("Greater Ghost");
        
        _cyclop.AddLink(new MarkovLinkEnemy      { Probability = 0.5f, State = _cyclop });
        _cyclop.AddLink(new MarkovLinkEnemy      { Probability = 0.3f, State = _bat });
        _cyclop.AddLink(new MarkovLinkEnemy      { Probability = 0.2f, State = _greaterGhost });

        _bat.AddLink(new MarkovLinkEnemy { Probability = 0.3f, State = _cyclop });
        _bat.AddLink(new MarkovLinkEnemy { Probability = 0.4f, State = _bat });
        _bat.AddLink(new MarkovLinkEnemy { Probability = 0.3f, State = _greaterGhost });

        _greaterGhost.AddLink(new MarkovLinkEnemy{ Probability = 0.2f, State = _cyclop });
        _greaterGhost.AddLink(new MarkovLinkEnemy{ Probability = 0.3f, State = _bat });
        _greaterGhost.AddLink(new MarkovLinkEnemy{ Probability = 0.5f, State = _greaterGhost });
    }
    
    public void SpawnEnemies(List<BoundsInt> rooms, List<RoomStereotype> roomSequence)
    {
        ClearEnemies();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (i >= roomSequence.Count || roomSequence[i] == null) continue;

            RoomType type = roomSequence[i].Type;
            if (type != RoomType.Fight && type != RoomType.BigFight) continue;

            int count = type == RoomType.BigFight
                ? Random.Range(_bigFightMin, _bigFightMax + 1)
                : Random.Range(_smallFightMin, _smallFightMax + 1);

            SpawnEnemiesInRoom(rooms[i], count);
        }
    }

    private void SpawnEnemiesInRoom(BoundsInt room, int count)
    {
        MarkovStateEnemy current = _cyclop;

        for (int i = 0; i < count; i++)
        {
            if (current == null) current = _cyclop;

            GameObject prefab = GetPrefab(current.Name);
            if (prefab == null) { current = current.NextEnemyState(); continue; }
            
            int x = Random.Range(room.xMin + 1, room.xMax - 1);
            int y = Random.Range(room.yMin + 1, room.yMax - 1);
            Vector3 pos = new Vector3(x + 0.5f, y + 0.5f, 0);

            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
            enemy.name = $"{current.Name}_{i}";
            _spawnedEnemies.Add(enemy);

            current = current.NextEnemyState();
        }
    }

    private GameObject GetPrefab(string enemyName)
    {
        return enemyName switch
        {
            "Cyclop"        => _cyclopPrefab,
            "Lesser Ghost"  => _batPrefab,
            "Greater Ghost" => _greaterGhostPrefab,
            _               => null
        };
    }

    public void ClearEnemies()
    {
        foreach (var obj in _spawnedEnemies)
            if (obj != null) DestroyImmediate(obj);
        _spawnedEnemies.Clear();
    }
}