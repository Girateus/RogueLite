using FSM;
using UnityEngine;

public class FsmGreaterGhost : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _detectionRange = 7f;
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private float _summonCooldown = 3f;
    [SerializeField] private int _maxMinions = 3;

    [Header("References")]
    [SerializeField] private GameObject _lesserGhostPrefab;

    private Machine _machine;
    private EnemyContext _ctx;

    private void Start()
    {
        Transform player = GameObject.FindWithTag("Player")?.transform;

        _ctx = new EnemyContext(transform, player)
        {
            DetectionRange = _detectionRange,
            MoveSpeed = _moveSpeed,
            MinionPrefab = _lesserGhostPrefab,
            SummonCooldown = _summonCooldown,
            MaxMinions = _maxMinions
        };
        
        var patrol = new PatrolState(_ctx);
        var flee   = new FleeState(_ctx);
        var summon = new SummonState(_ctx);

        _machine = new Machine();
        
       
        _machine.AddTransition(patrol, () => IsPlayerClose(), flee);
        _machine.AddTransition(summon, () => IsPlayerClose(), flee);
        
        _machine.AddTransition(flee, () => !IsPlayerClose(), summon);
        
        _machine.AddTransition(summon, () => !IsPlayerClose(), patrol);

        _machine.SetState(patrol);
    }

    private bool IsPlayerClose() 
    {
        if (_ctx.PlayerTransform == null) return false;
        return _ctx.DistanceToPlayer < _ctx.DetectionRange;
    }

    private void Update() => _machine?.Tick();
}