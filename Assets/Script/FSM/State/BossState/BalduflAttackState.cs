using FSM;
using UnityEngine;

public class BalduflAttackState : IState
{
    private BossContext _ctx;
   public DamageTaker _damageTaker;
    private float _attackCooldown;
    private float _attackRange;
    private float _hammerRadius;
    private Transform _hammerTransform; // le pivot du marteau
    private Animator _hammerAnimator;
    private float _timer;
    public bool AttackDone { get; private set; }

    public BalduflAttackState(
        BossContext ctx,
        float attackRange,
        float attackCooldown,
        float hammerRadius,
        Transform hammerTransform,
        Animator hammerAnimator)
    {
        _ctx             = ctx;
        _attackRange     = attackRange;
        _attackCooldown  = attackCooldown;
        _hammerRadius    = hammerRadius;
        _hammerTransform = hammerTransform;
        _hammerAnimator  = hammerAnimator;
    }

    public void Enter()
    {
        AttackDone = false;
        _timer     = _attackCooldown;
        
        _hammerAnimator.SetBool("BaldulfAttack", true);

        Debug.Log("[Baldufl] Attack !");
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= _attackCooldown / 2f)
            CheckHit();

        if (_timer <= 0f)
            AttackDone = true;
    }

    public void Exit()
    {
        _hammerAnimator.SetBool("BaldulfAttack", false);
    }

    private void CheckHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            _hammerTransform.position,
            _hammerRadius,
            LayerMask.GetMask("Default")
        );

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("[Baldufl] Joueur touché !");
            //_damageTaker.TakeDamage(10f);
            Debug.Log("hp -10");
            
        }
    }
}