using FSM;
using UnityEngine;

public class LancelotDashState : IState
{
    private BossContext _ctx;
    private float _dashTimer;
    private Vector2 _dashDir;

    public bool DashFinished { get; private set; }

    public LancelotDashState(BossContext ctx) { _ctx = ctx; }

    public void Enter()
    {
        DashFinished = false;
        _dashTimer = _ctx.DashDuration;
        _dashDir = (_ctx.PlayerTransform.position - _ctx.Transform.position).normalized;
        //_ctx.Animator.SetBool("IsAttacking", true);
        Debug.Log("[Lancelot] Dash !");
    }

    public void Tick()
    {
        _dashTimer -= Time.deltaTime;
        _ctx.Rb.linearVelocity = _dashDir * _ctx.DashSpeed;

        if (_dashTimer <= 0)
        {
            _ctx.Rb.linearVelocity = Vector2.zero;
            DashFinished = true;
        }
    }

    public void Exit()
    {
//        _ctx.Animator.SetBool("IsAttacking", false);
        _ctx.Rb.linearVelocity = Vector2.zero;
    }
}