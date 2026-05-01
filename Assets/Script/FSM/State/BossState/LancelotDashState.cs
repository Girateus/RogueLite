using FSM;
using UnityEngine;

public class LancelotDashState : IState
{
    private BossContext _ctx;
    private float _dashTimer;
    private Vector2 _dashDir;
    private Transform _weaponTransform;
    private float _attackRadius;
    private bool _hitChecked;
    public bool DashFinished { get; private set; }

    public LancelotDashState(BossContext ctx, Transform weaponTransform, float attackRadius)
    {
        _ctx             = ctx;
        _weaponTransform = weaponTransform;
        _attackRadius    = attackRadius;
    }

    public void Enter()
    {
        DashFinished = false;
        _hitChecked  = false;
        _dashTimer   = _ctx.DashDuration;
        _dashDir     = (_ctx.PlayerTransform.position - _ctx.Transform.position).normalized;
        _ctx._animator.SetBool("Attack", true);
    }

    public void Tick()
    {
        _dashTimer -= Time.deltaTime;
        _ctx.Rb.linearVelocity = _dashDir * _ctx.DashSpeed;

        // Hit au milieu du dash
        if (!_hitChecked && _dashTimer <= _ctx.DashDuration / 2f)
        {
            CheckHit();
            _hitChecked = true;
        }

        if (_dashTimer <= 0f)
        {
            _ctx.Rb.linearVelocity = Vector2.zero;
            DashFinished = true;
        }
    }

    public void Exit()
    {
        _ctx._animator.SetBool("Attack", false);
        _ctx.Rb.linearVelocity = Vector2.zero;
    }

    private void CheckHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            _weaponTransform.position,
            _attackRadius
        );

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("[Lancelot] Joueur touché !");
            // TODO : hit.GetComponent<PlayerStats>()?.TakeDamage(damage);
        }
    }
}
