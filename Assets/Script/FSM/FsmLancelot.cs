using UnityEngine;

public class FsmLancelot : BossController
{
    [SerializeField] private float _dashSpeed = 12f;
    [SerializeField] private float _dashDuration = 0.3f;
    [SerializeField] private float _dashCooldown = 2f;

    private LancelotDashState _dashState;
    private LancelotCooldownState _cooldownState;

    protected override void Awake()
    {
        base.Awake();
        _ctx.DashSpeed    = _dashSpeed;
        _ctx.DashDuration = _dashDuration;
        _ctx.DashCooldown = _dashCooldown;
        //_ctx.Animator     = GetComponent<Animator>();
    }

    protected override void SetupStates()
    {
        _dashState     = new LancelotDashState(_ctx);
        _cooldownState = new LancelotCooldownState(_ctx);
    }

    protected override void OnActivated()
    {
        _machine.AddTransition(_idleState,
            () => true, _dashState);
        
        _machine.AddTransition(_dashState,
            () => _dashState.DashFinished, _cooldownState);
        
        _machine.AddTransition(_cooldownState,
            () => _cooldownState.CooldownDone, _dashState);
    } 
}
