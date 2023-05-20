using System;
using Data.Addressable;
using Data.Dynamic.Location;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Infrastructure.GlobalStateMachine.States.MainMenu;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UI.MainLocation;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationState : State<GameInstance>
    {
        public MainLocationState(GameInstance context, IUIFactory uiFactory, ISaveLoadService saveLoadService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, IAbstractFactory abstractFactory,
            IPlayerFactory playerFactory) :
            base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
        }

        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;

        public override void Enter()
        {
            _saveLoadService.SaveProgress();
            SelectionLocationSettingChange();
            SettingMenu();

            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _saveLoadService.SaveProgress();
            _saveLoadInstancesWatcher.ClearProgress();

            _playerFactory.DestroyPlayer();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMenuInMainLocationScreen();
        }

        private void SelectionLocationSettingChange()
        {
            var progress = _saveLoadService.LoadProgress();
            progress.currentLocation.locationType = CurrentLocation.LocationType.Main;
            _saveLoadService.SaveProgress();
        }

        private void SettingMenu()
        {
            _uiFactory.MenuInMainLocationScreen.GetComponent<MenuInMainLocationScreen>()
                .SetUp(_saveLoadService.SaveProgress, ExitInMainMenu);
            
            void ExitInMainMenu()
            {
                Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.MAIN_MENU_SCENE_NAME,
                    typeof(MainMenuSetUpState));
            }
        }
    }
}