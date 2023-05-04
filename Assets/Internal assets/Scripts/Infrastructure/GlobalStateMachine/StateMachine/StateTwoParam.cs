namespace Infrastructure.GlobalStateMachine.StateMachine
{
    public class StateTwoParam<TContext, T0, T1> : BaseState<TContext>
    {
        public StateTwoParam(TContext context) : base(context)
        {
        }

        public virtual void Enter(T0 arg0, T1 arg1) {}
    }
}