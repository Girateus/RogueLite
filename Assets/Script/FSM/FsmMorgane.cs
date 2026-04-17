using UnityEngine;

public class FsmMorgane : BossController
{ 
    [SerializeField] private float _spellCooldown = 2f;
    [SerializeField] private float _minRange = 3f; 
    [SerializeField] private float _maxRange = 7f;  
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private GameObject _lightningPrefab;

    private MorganeFleeState _fleeState;
    private MorganeSpellState _spellState;

    protected override void Awake()
    {
        base.Awake();
        _ctx.SpellCooldown   = _spellCooldown;
        _ctx.AttackRange     = _maxRange;  
        _ctx.BubblePrefab    = _bubblePrefab;
        _ctx.LightningPrefab = _lightningPrefab;
    }

    protected override void SetupStates()
    {
        _fleeState  = new MorganeFleeState(_ctx);
        _spellState = new MorganeSpellState(_ctx);
    }

    protected override void OnActivated()
    {
       
        _machine.AddTransition(_idleState, () => true, _fleeState);
        
        _machine.AddTransition(_fleeState, () => _ctx.DistanceToPlayer >= _minRange && _ctx.DistanceToPlayer <= _ctx.AttackRange, _spellState);
        
        _machine.AddTransition(_spellState, () => _ctx.DistanceToPlayer < _minRange, _fleeState);
        
        _machine.AddTransition(_spellState, () => _spellState.SpellDone, _fleeState);
    }
}

