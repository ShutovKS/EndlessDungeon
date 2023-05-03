using Data.Addressable;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationLoadingState : StateOneParam<GameInstance, MainMenuScreen>
    {
        public MainLocationLoadingState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;
        private GameObject GameLoadingScreenInstance;
        private MainMenuScreen _mainMenuScreen;

        public override async void Enter(MainMenuScreen mainMenuScreen)
        {
            _mainMenuScreen = mainMenuScreen;

            await _uiFactory.CreateLoadingScreen();

            var asyncOperationHandle =
                Addressables.LoadSceneAsync((AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME));
            await asyncOperationHandle.Task;
            
            Context.StateMachine.SwitchState<MainLocationSetUpState, MainMenuScreen>(_mainMenuScreen);
        }
    }
}