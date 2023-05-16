using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateMachine<TContext>
    {
        private readonly Dictionary<Type, BaseState<TContext>> _states;

        private readonly float _tickRate = 0;

        private BaseState<TContext> _currentState { get; set; }

        private BaseState<TContext> CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                Debug.Log($"Set state {value.GetType().Name}");
            }
        }

        protected TContext Context;

        public StateMachine(TContext context, params BaseState<TContext>[] states)
        {
            Context = context;
            _states = new Dictionary<Type, BaseState<TContext>>(states.Length);

            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }

            TickAsync();
        }

        #region SwitchState

        public void SwitchState(Type type)
        {
            CurrentState?.Exit();

            var newState = _states[type] as State<TContext>;

            CurrentState = newState;

            newState?.Enter();
        }
        
        public void SwitchState<T0>(Type type, T0 arg0)
        {
            CurrentState?.Exit();

            var newState = _states[type] as StateOneParam<TContext, T0>;

            CurrentState = newState;

            newState?.Enter(arg0);
        }

        public void SwitchState<T0, T1>(Type type, T0 arg0, T1 arg1)
        {
            CurrentState?.Exit();

            var newState = _states[type] as StateTwoParam<TContext, T0, T1>;

            CurrentState = newState;

            newState?.Enter(arg0, arg1);
        }
        
        public void SwitchState<TState>() where TState : State<TContext>
        {
            CurrentState?.Exit();

            var newState = GetState<TState>();

            CurrentState = newState;

            newState?.Enter();
        }

        public void SwitchState<TState, T0>(T0 arg0) where TState : StateOneParam<TContext, T0>
        {
            CurrentState?.Exit();

            var newState = GetState<TState>();

            CurrentState = newState;

            newState.Enter(arg0);
        }

        public void SwitchState<TState, T0, T1>(T0 arg0, T1 arg1) where TState : StateTwoParam<TContext, T0, T1>
        {
            CurrentState?.Exit();

            var newState = GetState<TState>();

            CurrentState = newState;

            newState.Enter(arg0, arg1);
        }

        #endregion
        
        private async void TickAsync()
        {
            while (true)
            {
                if (_tickRate == 0)
                {
                    await Task.Yield();
                }
                else
                {
                    await Task.Delay((int)(_tickRate * 1000));
                }

                CurrentState?.Tick();
            }
        }

        private TState GetState<TState>() where TState : BaseState<TContext>
        {
            return _states[typeof(TState)] as TState;
        }
    }
}