using System;

namespace FSM
{
    public class Transition
    {
        private IState _state;
        private Func<bool> _condition;

        public Transition(Func<bool> condition, IState state)
        {
            _condition = condition;
            _state = state;
        }
        
        public bool IsVerified() => _condition();
        public IState State => _state;
    }
}