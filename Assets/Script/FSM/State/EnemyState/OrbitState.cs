using UnityEngine;

namespace FSM
{
    public class OrbitState : IState
    {
        private EnemyContext _ctx;

        public OrbitState(EnemyContext ctx) { _ctx = ctx; }

        public void Enter() { }

        public void Tick()
        {
            float dist = _ctx.DistanceToPlayer;
            float diff = dist - _ctx.PreferredDistance;

            Vector2 dir = (_ctx.Transform.position - _ctx.PlayerTransform.position).normalized;

            if (Mathf.Abs(diff) > 0.1f)
            {
                float sign = diff < 0 ? 1f : -1f;
                _ctx.Rb.linearVelocity = dir * sign * _ctx.MoveSpeed;
            }
            else
            {
                _ctx.Rb.linearVelocity = Vector2.zero;
            }
        }

        public void Exit()
        {
            _ctx.Rb.linearVelocity = Vector2.zero;
        }
    }
}