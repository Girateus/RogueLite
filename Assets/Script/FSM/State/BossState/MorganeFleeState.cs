using FSM;
using UnityEngine;

public class MorganeFleeState : IState
{
    private BossContext _ctx;
    public MorganeFleeState(BossContext ctx) { _ctx = ctx; }

    public void Enter() { }

    public void Tick()
    {
        Vector2 fleeDir = (_ctx.Transform.position - _ctx.PlayerTransform.position).normalized;
        _ctx.Rb.linearVelocity = fleeDir * 3f;
    }

    public void Exit() { _ctx.Rb.linearVelocity = Vector2.zero; }
}