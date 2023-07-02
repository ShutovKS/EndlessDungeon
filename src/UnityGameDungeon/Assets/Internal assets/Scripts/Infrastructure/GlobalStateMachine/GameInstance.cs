#region

using Data.LocationSettings;
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

#endregion

namespace Infrastructure.GlobalStateMachine
{
    public class GameInstance
    {
        public GameInstance(
            IAssetsAddressableService assetsAddressableService,
            PlayerStaticDefaultData playerStaticDefaultData,
            MainLocationSettings mainLocationSettings,
            DungeonRoomSettings dungeonRoomSettings,
            MainMenuSettings mainMenuSettings,
            IPersistentProgressService persistentProgressService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            ISaveLoadService saveLoadService,
            IAbstractFactory abstractFactory,
            IPlayerFactory playerFactory,
            IEnemyFactory enemyFactory,
            IUIFactory uiFactory)
        {
            StateMachine = new StateMachine<GameInstance>(
                new BootstrapState(
                    this),
                new LoadLastSavedLocationState(
                    this,
                    saveLoadService),
                new SceneLoadingState(
                    this,
                    uiFactory),
                new RemoveProgressDataState(
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
                    mainMenuSettings,
                    playerFactory),
                new MainMenuState(
                    this,
                    uiFactory,
                    abstractFactory,
                    saveLoadService,
                    playerFactory),
                new MainLocationSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData,
                    playerFactory),
                new MainLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    abstractFactory,
                    playerFactory,
                    persistentProgressService),
                new DungeonLocationGenerationState(
                    this,
                    persistentProgressService,
                    saveLoadService),
                new DungeonLocationSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    dungeonRoomSettings,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData,
                    enemyFactory,
                    playerFactory,
                    uiFactory),
                new DungeonLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    abstractFactory,
                    enemyFactory,
                    saveLoadInstancesWatcher,
                    playerFactory,
                    persistentProgressService)
            );

            StateMachine.SwitchState(typeof(BootstrapState));
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}
