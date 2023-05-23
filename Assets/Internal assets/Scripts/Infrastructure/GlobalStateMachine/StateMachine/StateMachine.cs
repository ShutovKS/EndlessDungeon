using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateMachine<TContext>
    {
        private readonly Dictionary<Type, BaseState<TContext>> _states;
        
        private BaseState<TContext> CurrentState { get; set; }
        
        protected float TickRate = 0;

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

        public void SwitchState<TState>() where TState : State<TContext>
        {
            CurrentState?.Exit();

            TickRate = 0;
            
            var newState = GetState<TState>();

            CurrentState = newState;

            newState?.Enter();
        }
        
        public void SwitchState(Type type)
        {
            CurrentState?.Exit();

            var newState = _states[type] as State<TContext>;

            CurrentState = newState;

            newState?.Enter();
        }

        public void SwitchState<TState, T0>(T0 arg0) where TState : StateWithParam<TContext, T0>
        {
            CurrentState?.Exit();

            var newState = GetState<TState>();

            CurrentState = newState;

            newState.Enter(arg0);
        }
        
        public void SwitchState<T0>(Type type, T0 arg0)
        {
            CurrentState?.Exit();

            var newState = _states[type] as StateWithParam<TContext, T0>;

            CurrentState = newState;

            newState?.Enter(arg0);
        }

        #endregion

        private async void TickAsync()
        {
            while (true)
            {
                if (TickRate == 0)
                {
                    await Task.Yield();
                }
                else
                {
                    await Task.Delay((int)(TickRate * 1000));
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