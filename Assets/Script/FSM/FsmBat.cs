using UnityEngine;
using FSM;

namespace FSM
{
    public class FsmBat : MonoBehaviour
    {  [SerializeField] private float _detectionRange = 5f;
        [SerializeField] private float _preferredDistance = 3f;
        [SerializeField] private float _moveSpeed = 2f;
        [SerializeField] private float _shootRange = 4f;
        [SerializeField] private float _shootCooldown = 1.5f;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _projectileSpeed = 6f;

        private Machine _machine;
        private EnemyContext _ctx;
        private BatShootState _shootState;

        private void Start()
        {
            Transform player = GameObject.FindWithTag("Player")?.transform;
            if (player == null) return;

            _ctx = new EnemyContext(transform, player)
            {
                DetectionRange    = _detectionRange,
                PreferredDistance = _preferredDistance,
                MoveSpeed         = _moveSpeed,
                AttackRange       = _shootRange,
                ProjectilePrefab  = _projectilePrefab,
                ProjectileSpeed   = _projectileSpeed,
                Rb                = GetComponent<Rigidbody2D>()
            };

            var patrol = new PatrolState(_ctx);
            var orbit  = new OrbitState(_ctx);
            _shootState = new BatShootState(_ctx, _shootCooldown);

            _machine = new Machine();
            _machine.SetState(patrol);
            
            _machine.AddTransition(patrol,
                () => _ctx.DistanceToPlayer < _ctx.DetectionRange, orbit);
            
            _machine.AddTransition(orbit,
                () => _ctx.DistanceToPlayer <= _ctx.AttackRange, _shootState);
            
            _machine.AddTransition(_shootState,
                () => _shootState.ShouldStopShooting, orbit);
            
            _machine.AddTransition(orbit,
                () => _ctx.DistanceToPlayer > _ctx.DetectionRange, patrol);
        }

        private void Update() => _machine?.Tick();

        private void OnDrawGizmos()
        {
            if (_ctx == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _ctx.DetectionRange);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _ctx.PreferredDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _ctx.AttackRange);
        }
    }
    
}