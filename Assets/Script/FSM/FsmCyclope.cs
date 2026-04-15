using FSM;
using UnityEngine;

namespace FSM
{
    public class FsmCyclope : MonoBehaviour
    {
        [SerializeField] private EnemyContext _context;
        
        private FSM.Machine _enemyMachine = new FSM.Machine();

        private FSM.IdleState _idle = new FSM.IdleState();
        private FSM.PatrolState _patrol = new FSM.PatrolState();
        private FSM.ChaseState _chase = new FSM.ChaseState();

        private void Start()
        {
            _idle.Context = _context;
            _patrol.Context = _context;
            
            _enemyMachine.AddTransition(_idle,() => true, _patrol);
            //_enemyMachine.AddTransition();
        }

        void Update()
        {
            _enemyMachine.Tick();
        }
        
    }
}