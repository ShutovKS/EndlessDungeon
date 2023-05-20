using System;
using Data.Addressable;
using Data.Dynamic.Location;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Infrastructure.GlobalStateMachine.States.MainMenu;
using Loot;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UI.DungeonRoom;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomState : State<GameInstance>
    {
        public DungeonRoomState(GameInstance context, IUIFactory uiFactory, ISaveLoadService saveLoadService,
            IAbstractFactory abstractFactory, IEnemyFactory enemyFactory,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, IPlayerFactory playerFactory,
            IPersistentProgressService persistentProgressService) : base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _abstractFactory = abstractFactory;
            _enemyFactory = enemyFactory;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerFactory = playerFactory;
            _persistentProgressService = persistentProgressService;
        }

        private readonly IEnemyFactory _enemyFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IPlayerFactory _playerFactory;
        private readonly IPersistentProgressService _persistentProgressService;

        public override void Enter()
        {
            SelectionLocationSettingChange();
            CreatedActionAllDeadEnemies();
            SettingMenu();
            
            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _saveLoadInstancesWatcher.ClearProgress();

            _playerFactory.DestroyPlayer();
            _enemyFactory.DestroyAllInstances();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMenuInDungeonRoomScreen();
        }

        private void SelectionLocationSettingChange()
        {
            var progress = _saveLoadService.LoadProgress();
            progress.currentLocation.locationType = CurrentLocation.LocationType.DungeonRoom;
            _saveLoadService.SaveProgress();
        }
        
        private void CreatedActionAllDeadEnemies()
        {
            _enemyFactory.AllDeadEnemies += Finish;
            
            void Finish()
            {
                _persistentProgressService.Progress.dungeonRoom.seed = 0;
                _persistentProgressService.Progress.dungeonRoom.roomPassedCount++;
                _saveLoadService.SaveProgress();

                Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME,
                    typeof(DungeonRoomGenerationState));
            }

        }

        private void SettingMenu()
        {
            _uiFactory.MenuInDungeonRoomScreen.GetComponent<MenuInDungeonRoomScreen>().SetUp(ExitInMainLocation);
            
            void ExitInMainLocation()
            {
                Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME,
                    typeof(MainLocationSetUpState));
            }

        }
    }
}