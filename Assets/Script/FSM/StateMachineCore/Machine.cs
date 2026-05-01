using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    
    public class Machine
    {
        private IState _currentState;
        
        private List<Transition> _anyStateTransitions = new List<Transition>();
        
        private Dictionary< IState, List<Transition>> _transition = new Dictionary< IState, List<Transition>>();

        public void SetState(IState state)
        {
            if (state == null) return;
            
            Debug.Log($"Exiting State : {_currentState?.GetType()}");
            _currentState?.Exit();
            _currentState = state;
            Debug.Log($"Entering State : {_currentState.GetType()}");
            _currentState.Enter();
        }
        
        public void Tick()
        {
            IState newState = CheckTransitions();
            
            if (newState != null && newState != _currentState)
            {
                SetState(newState);
            }
    
            _currentState?.Tick();
        }

        public void AddAnyStateTransition(Func<bool> condition,  IState state)
        {
            _anyStateTransitions.Add(new Transition(condition, state));
        }

        public void AddTransition(IState fromState, Func<bool> condition, IState toState)
        {
            if (_transition.TryGetValue(fromState, out List<Transition> transitionsList))
            {
             transitionsList.Add(new Transition(condition, toState));
            }
            else
            {
                List<Transition> newList = new List<Transition>();
                newList.Add(new Transition(condition, toState));
                _transition.Add(fromState, newList);
                
            }
        }
        
        private IState CheckTransitions()
        {
            foreach (var anyStateTransition in _anyStateTransitions)
            {
                if(anyStateTransition.IsVerified()) return anyStateTransition.State;
            }

            if (_currentState != null)
            {
                if (_transition.TryGetValue(_currentState, out List<Transition> possibleTransitions))
                {
                    foreach (Transition possibleTransition in possibleTransitions)
                    {
                        if (possibleTransition.IsVerified()) return possibleTransition.State;
                    }
                }
            }

            return _currentState;
        }

        
    }
}