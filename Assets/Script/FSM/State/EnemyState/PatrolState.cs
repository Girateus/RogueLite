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
            
            Vector2 currentPos = _ctx.Transform.position;
            
            Vector2 nextPos = Vector2.MoveTowards(currentPos, _targetPoint, _ctx.MoveSpeed * Time.deltaTime);
            
            _ctx.Transform.position = new Vector3(nextPos.x, nextPos.y, _ctx.Transform.position.z);

            if (Vector2.Distance(currentPos, _targetPoint) < 0.1f)
            {
                _waiting = true;
                _waitTimer = _waitTime;
            }
        }

        public void Exit() { }

        private void PickNewTarget()
        {
            Vector2 offset = Random.insideUnitCircle * 3f;
            Vector2 currentPos2D = new Vector2(_ctx.Transform.position.x, _ctx.Transform.position.y);
            _targetPoint = currentPos2D + offset;
        }
    }
}