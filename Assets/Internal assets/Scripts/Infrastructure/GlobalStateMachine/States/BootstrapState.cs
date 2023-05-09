using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.MainMenu;
using Zenject;

namespace Infrastructure.GlobalStateMachine.States
{
    public class BootstrapState : State<GameInstance>, IInitializable
    {
        public BootstrapState(GameInstance context) : base(context)
        {
        }

        public void Initialize()
        {
            Context.StateMachine.SwitchState<MainMenuLoadingState>();
        }
    }
}