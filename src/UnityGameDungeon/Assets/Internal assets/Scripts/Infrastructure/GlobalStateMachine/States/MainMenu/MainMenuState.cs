#region

using System;
using Data.Addressable;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Services.SaveLoad;
using UI.MainMenu;

#endregion

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuState : State<GameInstance>
    {
        public MainMenuState(
            GameInstance context,
            IUIFactory uiFactory,
            IAbstractFactory abstractFactory,
            ISaveLoadService saveLoadService,
            IPlayerFactory playerFactory) : base(context)
        {
            _abstractFactory = abstractFactory;
            _saveLoadService = saveLoadService;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;

        public override void Enter()
        {
            SettingMenu();

            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _uiFactory.DestroyMainMenuScreen();
            _abstractFactory.DestroyAllInstances();
            _playerFactory.DestroyPlayer();
        }

        private void SettingMenu()
        {
            if (_uiFactory.MainMenuScreen.TryGetComponent<MainMenuScreen>(out var mainMenuScreenComponent))
                mainMenuScreenComponent.SetUp(GoToNewGame, GoToLoadGame, _saveLoadService.IsInStockSave());
        }

        private void GoToNewGame()
        {
            Context.StateMachine.SwitchState<(string sceneName, Type nextStateType)>(
                typeof(RemoveProgressDataState),
                (AssetsAddressableConstants.MAIN_LOCATION_SCENE_NAME, typeof(MainLocationSetUpState)));
        }

        private void GoToLoadGame()
        {
            Context.StateMachine.SwitchState(typeof(LoadLastSavedLocationState));
        }
    }
}
