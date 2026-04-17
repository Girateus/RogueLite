using FSM;
using UnityEngine;

namespace FSM
{
    public class FsmCyclope : MonoBehaviour
    {
        [SerializeField] private float _detectionRange = 6f;
        [SerializeField] private float _attackRange = 1.2f;
        [SerializeField] private float _moveSpeed = 3f;

        private Machine _machine;
        private EnemyContext _ctx;

        private void Start()
        {
            Transform player = GameObject.FindWithTag("Player")?.transform;

            _ctx = new EnemyContext(transform, player)
            {
                DetectionRange = _detectionRange,
                AttackRange = _attackRange,
                MoveSpeed = _moveSpeed
            };

            var patrol = new PatrolState(_ctx);
            var chase  = new ChaseState(_ctx);
            // var attack = new AttackState(_ctx); // Future idea

            _machine = new Machine();
            
            _machine.AddTransition(patrol, () => PlayerInSight(), chase);
            _machine.AddTransition(chase,  () => !PlayerInSight(), patrol);

            _machine.SetState(patrol);
        }

        private bool PlayerInSight()
        {
            if (_ctx.PlayerTransform == null) return false;
            return _ctx.DistanceToPlayer < _ctx.DetectionRange;
        }

        private void Update() => _machine?.Tick();
        
        private void OnDrawGizmos()
        {
            if (_ctx == null) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _ctx.DetectionRange);

            // Attack range en rouge
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _ctx.AttackRange);
            
        }
    }
}