namespace FSM
{
    public class ChaseState : IState
    {
        public void Enter()
        {
            
        }

        public void Tick()
        {
           // Context.DistanceToPlayer
        }

        public void Exit()
        {
        }

        public EnemyContext Context { get; set; }
    }
}