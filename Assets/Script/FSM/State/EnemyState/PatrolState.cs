using UnityEngine;

namespace FSM
{
    public class PatrolState : IState
    {
        private EnemyContext _ctx;
        private Vector2 _targetPoint;
        private float _waitTime = 2f;
        private float _waitTimer;
        private bool _waiting;

        public PatrolState(EnemyContext ctx)
        {
            _ctx = ctx;
        }

        public void Enter()
        {
            Debug.Log($"[Patrol] Enter — Room : {_ctx.Room} | min:{_ctx.Room.min} max:{_ctx.Room.max}");
            PickNewTarget();
        }

        public void Tick()
        {
            Debug.Log($"[Patrol] Tick — pos:{_ctx.Transform.position} target:{_targetPoint} rb:{_ctx.Rb?.linearVelocity}");
            if (_waiting)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0) { _waiting = false; PickNewTarget(); }
                return;
            }
            
            Vector2 direction = (_targetPoint - (Vector2)_ctx.Transform.position).normalized;
            
            if (_ctx.Rb != null) 
            {
                _ctx.Rb.linearVelocity = direction * _ctx.MoveSpeed;
            }
            
            if (Vector2.Distance(_ctx.Transform.position, _targetPoint) < 0.2f)
            {
                if (_ctx.Rb != null) _ctx.Rb.linearVelocity = Vector2.zero; // Stop
                _waiting = true;
                _waitTimer = _waitTime;
            }
        }

        public void Exit() { }

        private void PickNewTarget()
        {
            Vector2 randomOffset = Random.insideUnitCircle * 3f;
            _targetPoint = (Vector2)_ctx.Transform.position + randomOffset;
        }
    }
}