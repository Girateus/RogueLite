
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : MonoBehaviour
{
    private GameObject _bossPrefab;
    private GameObject _endCanvas;
    private GameObject _spawnedBoss;
    private bool _triggered = false;
    private AudioManager _audioManager;
    
    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void SetRefs(GameObject bossPrefab, GameObject endCanvas)
    {
        _bossPrefab = bossPrefab;
        _endCanvas  = endCanvas;
    }

    public void Init(BoundsInt room)
    {
        _audioManager = FindObjectOfType<AudioManager>();
        // Trigger de détection
        var col    = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size   = new Vector2(room.size.x, room.size.y);
        col.offset = new Vector2(
            room.center.x - transform.position.x,
            room.center.y - transform.position.y
        );
        
        if (_bossPrefab != null)
        {
            Vector3 center = new Vector3(room.center.x, room.center.y, 0);
            _spawnedBoss = Instantiate(_bossPrefab, center, Quaternion.identity);

            DamageTaker dt = _spawnedBoss.GetComponent<DamageTaker>();
            if (dt != null)
                dt._onDeath.AddListener(OnBossDied);
        }

        if (_endCanvas != null)
            _endCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;
        
        _spawnedBoss?.GetComponent<BossController>()?.OnPlayerEnterRoom();
        
        DamageTaker dt = _spawnedBoss?.GetComponent<DamageTaker>();
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (dt != null && hud != null)
        {
            string bossName = _bossPrefab.name;
            hud.SetBossDamageTaker(dt, bossName);
            _audioManager.PlayBossMusic();
        }

        Debug.Log("[BossRoom] Boss activé !");
    }

    private void OnBossDied()
    {
        Debug.Log("[BossRoom] Boss mort → générique !");
        if (_endCanvas != null)
            _endCanvas.SetActive(true);
    }
}