using Data.Settings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
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
        public GameInstance(IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService,
            IAbstractFactory abstractFactory,
            MainLocationSettings mainLocationSettings,
            MainMenuSettings mainMenuSettings,
            PlayerStaticDefaultData playerStaticDefaultData,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService,
            IEnemyFactory enemyFactory)
        {
            StateMachine = new StateMachine<GameInstance>(
                this,
                new BootstrapState(
                    this),
                new LoadLateLocation(
                    this,
                    saveLoadService),
                new ProgressLoadingState(
                    this,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    persistentProgressService),
                new MainMenuSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainMenuSettings),
                new MainMenuState(
                    this,
                    uiFactory,
                    abstractFactory,
                    saveLoadService),
                new SceneLoadingState(
                    this,
                    uiFactory),
                new MainLocationSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData),
                new MainLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    abstractFactory),
                new RemoveGameplayData(
                    this,
                    saveLoadService),
                new DungeonRoomGenerationState(
                    this,
                    persistentProgressService,
                    saveLoadService),
                new DungeonRoomSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData),
                new DungeonRoomSetUpNavMeshState(
                    this,
                    abstractFactory),
                new DungeonRoomSetUpEnemyState(
                    this,
                    enemyFactory),
                new DungeonRoomState(
                    this,
                    uiFactory,
                    saveLoadService,
                    abstractFactory,
                    enemyFactory,
                    saveLoadInstancesWatcher)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}