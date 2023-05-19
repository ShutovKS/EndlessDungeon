using Data.Settings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Infrastructure.GlobalStateMachine.States.MainMenu;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;

namespace Infrastructure.GlobalStateMachine
{
    public class GameInstance
    {
        public GameInstance(
            IPersistentProgressService persistentProgressService,
            IAssetsAddressableService assetsAddressableService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            PlayerStaticDefaultData playerStaticDefaultData,
            MainLocationSettings mainLocationSettings,
            DungeonRoomSettings dungeonRoomSettings,
            MainMenuSettings mainMenuSettings,
            IAbstractFactory abstractFactory,
            ISaveLoadService saveLoadService,
            IPlayerFactory playerFactory,
            IEnemyFactory enemyFactory,
            IUIFactory uiFactory)
        {
            StateMachine = new StateMachine<GameInstance>(
                this,
                new BootstrapState(this),
                new LoadLateLocation(this, saveLoadService),
                new SceneLoadingState(this, uiFactory),
                new RemoveProgressData(this, saveLoadService),
                new ProgressLoadingState(this, saveLoadService, saveLoadInstancesWatcher, persistentProgressService),
                new MainMenuSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainMenuSettings,
                    playerFactory),
                new MainMenuState(this, uiFactory, abstractFactory, saveLoadService, playerFactory),
                new MainLocationSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData,
                    playerFactory),
                new MainLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    abstractFactory,
                    playerFactory),
                new DungeonRoomGenerationState(this, persistentProgressService, saveLoadService),
                new DungeonRoomSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    dungeonRoomSettings,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData,
                    enemyFactory,
                    playerFactory),
                new DungeonRoomState(
                    this,
                    uiFactory,
                    saveLoadService,
                    abstractFactory,
                    enemyFactory,
                    saveLoadInstancesWatcher,
                    playerFactory)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}