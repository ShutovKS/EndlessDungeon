namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateWithParam<TContext, T0> : BaseState<TContext>
    {
        public StateWithParam(TContext context) : base(context) { }
        public virtual void Enter(T0 arg0) {}
    }
}