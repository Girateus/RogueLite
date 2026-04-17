using FSM;
using UnityEngine;

public class BalduflAttackState : IState
{
    private BossContext _ctx;
    private float _timer;
    public bool AttackDone { get; private set; }

    public BalduflAttackState(BossContext ctx) { _ctx = ctx; }

    public void Enter()
    {
        AttackDone = false;
        _timer = _ctx.AttackCooldown;
        Debug.Log("[Baldufl] SMASH !");
        // next phase add damage
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0) AttackDone = true;
    }

    public void Exit() { }
}