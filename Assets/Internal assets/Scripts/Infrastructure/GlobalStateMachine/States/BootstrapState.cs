using System;
using Data.Addressable;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
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
            Context.StateMachine.SwitchState<SceneLoadingState, (string sceneName, Type newStateType)>(
                (AssetsAddressablesConstants.MAIN_MENU_SCENE_NAME, typeof(MainMenuSetUpState)));
        }
    }
}