using System;
namespace Units.Enemy.State_Machines
{
    public class Transition
    {
        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }

        public Func<bool> Condition { get; }
        public IState To { get; }
    }
}
