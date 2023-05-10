namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateOneParam<TContext, T0> : BaseState<TContext>
    {
        public StateOneParam(TContext context) : base(context)
        {
        }

        public virtual void Enter(T0 mapDungeon) {}
    }
}