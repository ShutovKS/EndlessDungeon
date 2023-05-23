namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class State<TContext> : BaseState<TContext>
    {
        public State(TContext context) : base(context)
        {
        }

        public virtual void Enter()
        {
        }
    }
}
