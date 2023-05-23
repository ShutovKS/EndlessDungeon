namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class BaseState<TContext>
    {
        protected readonly TContext Context;

        public BaseState(TContext context)
        {
            Context = context;
        }
        public virtual void Tick() { }
        public virtual void Exit() { }
    }
}