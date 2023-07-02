#region

using System;
using Data.Addressable;
using Data.Dynamic.Location;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Infrastructure.GlobalStateMachine.States.MainMenu;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UI.MainLocation;
using UnityEngine;

#endregion

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationState : State<GameInstance>
    {
        public MainLocationState(
            GameInstance context,
            IUIFactory uiFactory,
            ISaveLoadService saveLoadService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IAbstractFactory abstractFactory,
            IPlayerFactory playerFactory,
            IPersistentProgressService persistentProgressService) : base(context)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _abstractFactory = abstractFactory;
            _saveLoadService = saveLoadService;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAbstractFactory _abstractFactory;

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IPlayerFactory _playerFactory;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;

        public override void Enter()
        {
            ChangeCurrentSaveLocation();
            SettingMenu();

            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _saveLoadService.SaveProgress();
            _saveLoadInstancesWatcher.ClearProgressWatchers();

            _playerFactory.DestroyPlayer();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroySkillsBookScreen();
            _uiFactory.DestroyMenuInMainLocationScreen();
        }

        private void ChangeCurrentSaveLocation()
        {
            var progress = _persistentProgressService.Progress;
            progress.currentLocation.locationType = CurrentLocation.LocationType.Main;
            _persistentProgressService.SetProgress(progress);
            _saveLoadService.SaveProgress();
        }

        private void SettingMenu()
        {
            _uiFactory.MenuInMainLocationScreen.GetComponent<MenuInMainLocationScreen>()
                .SetUp(_saveLoadService.SaveProgress, ExitInMainMenu);

            void ExitInMainMenu()
            {
                Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                    typeof(SceneLoadingState),
                    (AssetsAddressableConstants.MAIN_MENU_SCENE_NAME, typeof(MainMenuSetUpState)));
            }
        }
    }
}
