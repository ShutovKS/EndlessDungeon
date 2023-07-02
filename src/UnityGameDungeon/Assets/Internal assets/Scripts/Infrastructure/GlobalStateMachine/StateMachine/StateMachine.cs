#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateMachine<TContext>
    {
        public StateMachine(params BaseState<TContext>[] states)
        {
            _states = new Dictionary<Type, BaseState<TContext>>(states.Length);

            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }

            TickAsync();
        }

        private BaseState<TContext> CurrentState { get; set; }

        private readonly Dictionary<Type, BaseState<TContext>> _states;

        protected float TickRate;

        public void SwitchState(Type type)
        {
            CurrentState?.Exit();

            TickRate = 0;

            var newState = _states[type] as State<TContext>;

            CurrentState = newState;

            newState?.Enter();
        }

        public void SwitchState<T0>(Type type, T0 arg0)
        {
            CurrentState?.Exit();

            TickRate = 0;

            var newState = _states[type] as StateWithParam<TContext, T0>;

            CurrentState = newState;

            newState?.Enter(arg0);
        }

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
    }
}
