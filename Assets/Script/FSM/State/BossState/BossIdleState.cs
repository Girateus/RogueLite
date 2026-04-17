using FSM;
using UnityEngine;

public class BossIdleState : IState
{
    private BossContext _ctx;

    public BossIdleState(BossContext ctx) { _ctx = ctx; }

    public void Enter()
    {
        _ctx.Rb.linearVelocity = Vector2.zero;
        Debug.Log("[Boss] Idle — attend le joueur");
    }

    public void Tick() { } 

    public void Exit()
    {
        Debug.Log("[Boss] Activation !");
    }
}