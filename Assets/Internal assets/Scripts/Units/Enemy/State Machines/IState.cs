namespace Units.Enemy.State_Machines
{
    public interface IState
    {
        void OnEnter();
        void Tick();
        void OnExit();
    }
}
