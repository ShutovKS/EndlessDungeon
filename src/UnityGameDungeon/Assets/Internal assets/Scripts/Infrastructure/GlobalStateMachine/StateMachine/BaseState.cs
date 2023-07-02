namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class BaseState<TContext>
    {
        public BaseState(TContext context)
        {
            Context = context;
        }

        protected readonly TContext Context;

        public virtual void Tick()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
