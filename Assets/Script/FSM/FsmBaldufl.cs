using UnityEngine;

public class FsmBaldufl : BossController
{
    [SerializeField] private float _attackRange    = 1.5f;
    [SerializeField] private float _attackCooldown = 2.5f;
    [SerializeField] private float _hammerRadius   = 0.8f;
    
    [SerializeField] private Transform _hammerTransform;

    private BalduflChaseState  _chaseState;
    private BalduflAttackState _attackState;

    protected override void Awake() => base.Awake();

    protected override void SetupStates()
    {
        Animator hammerAnimator = _hammerTransform.GetComponent<Animator>();

        _chaseState  = new BalduflChaseState(_ctx);
        _attackState = new BalduflAttackState(
            _ctx,
            _attackRange,
            _attackCooldown,
            _hammerRadius,
            _hammerTransform,
            hammerAnimator
        );
    }
    protected override void Update()
    {
        base.Update();
        FlipSprite();
    }

    protected override void OnActivated()
    {
        _machine.AddTransition(_idleState,
            () => true, _chaseState);

        _machine.AddTransition(_chaseState,
            () => _ctx.DistanceToPlayer <= _attackRange, _attackState);

        _machine.AddTransition(_attackState,
            () => _attackState.AttackDone, _chaseState);
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (_hammerTransform == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hammerTransform.position, _hammerRadius);
    }
    
    private void FlipSprite()
    {
            if (_ctx?.PlayerTransform == null) return;

            float dirX = _ctx.PlayerTransform.position.x - transform.position.x;

            if (dirX > 0.1f)
                transform.localScale = new Vector3(1, 1, 1);
            else if (dirX < -0.1f)
                transform.localScale = new Vector3(-1, 1, 1);
        
    }
}
