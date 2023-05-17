using System;
using Data.Addressable;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using UI.MainMenu;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuState : StateOneParam<GameInstance, GameObject>
    {
        public MainMenuState(GameInstance context, IUIFactory uiFactory, IAbstractFactory abstractFactory,
            ISaveLoadService saveLoadService) : base(
            context)
        {
            _uiFactory = uiFactory;
            _abstractFactory = abstractFactory;
            _saveLoadService = saveLoadService;
        }

        private readonly IUIFactory _uiFactory;
        private readonly IAbstractFactory _abstractFactory;
        private readonly ISaveLoadService _saveLoadService;

        public override void Enter(GameObject mainMenuScreen)
        {
            _uiFactory.DestroyLoadingScreen();

            if (mainMenuScreen.TryGetComponent<MainMenuScreen>(out var mainMenuScreenComponent))
                mainMenuScreenComponent.SetUp(GoToNewGame, GoToLoadGame, _saveLoadService.IsInStockSave());
        }

        public override void Exit()
        {
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