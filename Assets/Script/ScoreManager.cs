using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private SO_IntValue _score;
    [SerializeField] private int _pointsPerEnemy = 100;
    [SerializeField] private int _pointsPerBoss  = 1000;
    private TMP_Text _text;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        _score.Value = 0; // reset au lancement
    }
    
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void AddEnemyScore()  => _score.Value += _pointsPerEnemy;
    public void AddBossScore()   => _score.Value += _pointsPerBoss;
    
    void Update()
    {
        _text.text = _score.Value.ToString("000000");
    }
}