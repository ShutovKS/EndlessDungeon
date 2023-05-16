using System;
using Data.Addressable;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UI.MainMenu;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuState : StateOneParam<GameInstance, GameObject>
    {
        public MainMenuState(GameInstance context, IUIFactory uiFactory, IAbstractFactory abstractFactory) : base(
            context)
        {
            _uiFactory = uiFactory;
            _abstractFactory = abstractFactory;
        }

        private readonly IUIFactory _uiFactory;
        private readonly IAbstractFactory _abstractFactory;
        private MainMenuScreen _mainMenuScreen;

        public override void Enter(GameObject mainMenuScreen)
        {
            _uiFactory.DestroyLoadingScreen();

            if (mainMenuScreen.TryGetComponent<MainMenuScreen>(out var mainMenuScreenComponent))
            {
                _mainMenuScreen = mainMenuScreenComponent;
                _mainMenuScreen.SetUp(GoToNewGame, GoToLoadGame);
            }
        }

        public override void Exit()
        {
            if (_mainMenuScreen != null)
                _mainMenuScreen.ClearAction();

            _uiFactory.DestroyMainMenuScreen();
            _abstractFactory.DestroyAllInstances();
        }

        private void GoToNewGame()
        {
            Context.StateMachine.SwitchState<RemoveGameplayData>();
        }

        private void GoToLoadGame()
        {
            Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME,
                typeof(MainLocationSetUpState));
        }
    }
}