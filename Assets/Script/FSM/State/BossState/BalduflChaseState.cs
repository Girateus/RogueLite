using FSM;
using UnityEngine;

public class BalduflChaseState : IState
{
    private BossContext _ctx;
    public BalduflChaseState(BossContext ctx) { _ctx = ctx; }

    public void Enter() { }

    public void Tick()
    {
        Vector2 dir = (_ctx.PlayerTransform.position - _ctx.Transform.position).normalized;
        _ctx.Rb.linearVelocity = dir * 1.5f;
    }

    public void Exit() { _ctx.Rb.linearVelocity = Vector2.zero; }
}