using UnityEngine;

namespace FSM
{
    public class ChaseState : IState
    {
        private EnemyContext _ctx;

        public ChaseState(EnemyContext ctx) { _ctx = ctx; }

        public void Enter() { }

        public void Tick()
        {
            _ctx.Transform.position = Vector2.MoveTowards(
                _ctx.Transform.position,
                _ctx.PlayerTransform.position,
                _ctx.MoveSpeed * Time.deltaTime
            );
        }

        public void Exit() { }
    }
}