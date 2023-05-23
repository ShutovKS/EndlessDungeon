#region

using System;
using System.Collections.Generic;

#endregion

namespace Units.Enemy.State_Machines
{
    public class StateMachine
    {
        private readonly static List<Transition> EmptyTransitions = new List<Transition>(0);

        private readonly List<Transition> _anyTransitions = new List<Transition>();

        private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

        private IState _currentState;

        private List<Transition> _currentTransitions = new List<Transition>();

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null) SetState(transition.To);
    
            _currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;
            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
            _currentTransitions ??= EmptyTransitions;

            _currentState.OnEnter();
        }

        public void AddTransition(IState to, IState from, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }
    }
}
