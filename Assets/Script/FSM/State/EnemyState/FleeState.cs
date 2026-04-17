using UnityEngine;

namespace FSM
{
    public class FleeState : IState
    {
        
        private EnemyContext _ctx;

        public FleeState(EnemyContext ctx) { _ctx = ctx; }

        public void Enter() { }

        public void Tick()
        {
            Vector2 fleeDir = (_ctx.Transform.position - _ctx.PlayerTransform.position).normalized;
            _ctx.Transform.position += (Vector3)(fleeDir * _ctx.MoveSpeed * Time.deltaTime);
        }

        public void Exit() { }
    }
}