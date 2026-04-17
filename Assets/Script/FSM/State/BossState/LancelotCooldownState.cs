using FSM;
using UnityEngine;

public class LancelotCooldownState : IState
{
    private BossContext _ctx;
    private float _timer;
    public bool CooldownDone { get; private set; }

    public LancelotCooldownState(BossContext ctx) { _ctx = ctx; }

    public void Enter() { CooldownDone = false; _timer = _ctx.DashCooldown; }
    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0) CooldownDone = true;
    }
    public void Exit() { }
}