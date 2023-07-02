#region

using System;
using Data.Addressable;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UI.DungeonRoom;

#endregion

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonLocationState : State<GameInstance>
    {
        public DungeonLocationState(GameInstance context,
            IUIFactory uiFactory,
            ISaveLoadService saveLoadService,
            IAbstractFactory abstractFactory,
            IEnemyFactory enemyFactory,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPlayerFactory playerFactory,
            IPersistentProgressService persistentProgressService) : base(context)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _saveLoadService = saveLoadService;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IEnemyFactory _enemyFactory;

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IPlayerFactory _playerFactory;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;

        public override void Enter()
        {
            CreatedActionAllDeadEnemies();
            SettingMenu();

            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _persistentProgressService.Progress.dungeonLocation.seed = 0;
            _saveLoadService.SaveProgress();
            _saveLoadInstancesWatcher.ClearProgressWatchers();

            _playerFactory.DestroyPlayer();
            _enemyFactory.DestroyAllInstances();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMenuInDungeonRoomScreen();
        }

        private void CreatedActionAllDeadEnemies()
        {
            _enemyFactory.AllDeadEnemies += Finish;

            void Finish()
            {
                _persistentProgressService.Progress.dungeonLocation.roomPassedCount++;

                Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                    typeof(SceneLoadingState),
                    (AssetsAddressableConstants.DUNGEON_ROOM_SCENE_NAME, typeof(DungeonLocationGenerationState)));
            }
        }

        private void SettingMenu()
        {
            _uiFactory.MenuInDungeonRoomScreen.GetComponent<MenuInDungeonLocationScreen>().SetUp(ExitInMainLocation);

            void ExitInMainLocation()
            {
                _persistentProgressService.Progress.dungeonLocation.roomPassedCount = 0;

                Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                    typeof(SceneLoadingState),
                    (AssetsAddressableConstants.MAIN_LOCATION_SCENE_NAME, typeof(MainLocationSetUpState)));
            }
        }
    }
}
