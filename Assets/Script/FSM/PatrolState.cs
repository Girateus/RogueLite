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
            PickNewTarget();
        }

        public void Tick()
        {
            if (_waiting)
            {
                _waitTimer -= Time.deltaTime;
                if (_waitTimer <= 0) { _waiting = false; PickNewTarget(); }
                return;
            }
            
            Vector2 pos = _ctx.Transform.position;
            _ctx.Transform.position = Vector2.MoveTowards(pos, _targetPoint, _ctx.MoveSpeed * Time.deltaTime);

            if (Vector2.Distance(pos, _targetPoint) < 0.1f)
            {
                _waiting = true;
                _waitTimer = _waitTime;
            }
        }

        public void Exit() { }
        public EnemyContext Context { get; set; }

        private void PickNewTarget()
        {
            Vector2 offset = Random.insideUnitCircle * 3f;
            _targetPoint = (Vector2)_ctx.Transform.position + offset;
        }
    }
}