using UnityEngine;
using UnityEngine.Events;

public class DamageTaker : MonoBehaviour
{
    [SerializeField] private float _hpMax;
    [SerializeField] private bool _destroyable = true;

    [Header("Events")]
    [SerializeField] public UnityEvent _onDeath;
    [SerializeField] private UnityEvent _gameOver;
    public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();
    
    [Header("Score")]
    [SerializeField] private bool _isBoss = false;

    public float HpMax => _hpMax;
    public float _hp;

    private EnemyHitReaction _hitReaction;

    private void Start()
    {
        _hitReaction = GetComponent<EnemyHitReaction>();
        _hp = _hpMax;
        OnHealthChanged.Invoke(_hp);
    }

    public void TakeDamage(float damage, Vector2 hitDirection = default)
    {
        _hp -= damage;
        OnHealthChanged.Invoke(_hp);
        _hitReaction?.ReactToHit(hitDirection);

        if (_hp <= 0)
            Die();
    }

    public void Heal(float heal)
    {
        _hp = Mathf.Min(_hp + heal, _hpMax);
        OnHealthChanged.Invoke(_hp);
    }

    private void Die()
    {
        _onDeath.Invoke();
        
        if (ScoreManager.Instance != null)
        {
            if (_isBoss) ScoreManager.Instance.AddBossScore();
            else         ScoreManager.Instance.AddEnemyScore();
        }

        if (_destroyable)
            Destroy(gameObject);
        else
            _gameOver.Invoke();
    }
}