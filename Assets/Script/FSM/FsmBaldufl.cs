using UnityEngine;

public class FsmBaldufl : BossController
{
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackCooldown = 2.5f;

    private BalduflChaseState _chaseState;
    private BalduflAttackState _attackState;

    protected override void Awake()
    {
        base.Awake();
        _ctx.AttackRange   = _attackRange;
        _ctx.AttackCooldown = _attackCooldown;
    }

    protected override void SetupStates()
    {
        _chaseState  = new BalduflChaseState(_ctx);
        _attackState = new BalduflAttackState(_ctx);
    }

    protected override void OnActivated()
    {
        _machine.AddTransition(_idleState, () => true, _chaseState);
        
        _machine.AddTransition(_chaseState, () => _ctx.DistanceToPlayer <= _ctx.AttackRange, _attackState);
        
        _machine.AddTransition(_attackState, () => _attackState.AttackDone, _chaseState);
    }
}
