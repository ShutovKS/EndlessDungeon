using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine
{
    public class GameInstance
    {
        public GameInstance(IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService,
            IAbstractFactory abstractFactory,
            MainLocationSettings mainLocationSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService,
            IEnemyFactory enemyFactory)
        {
            StateMachine = new StateMachine<GameInstance>(
                this,
                new BootstrapState(this),
                new MainMenuState(this, uiFactory),
                new SceneLoadingForMainLocationState(this, uiFactory),
                new MainLocationLoadingState(this, uiFactory),
                new SceneLoadingForDungeonRoomState(this, uiFactory),
                new DungeonRoomLoadingState(this, uiFactory),
                new DungeonRoomGenerationState(this),
                new DungeonRoomState(this, uiFactory),
                new DungeonRoomSetUpNavMeshState(this, abstractFactory),
                new DungeonRoomSetUpEnemyState(this, enemyFactory),
                new MainLocationSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadInstancesWatcher),
                new ProgressLoadingForMainState(
                    this,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    persistentProgressService),
                new MainLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    abstractFactory),
                new DungeonRoomSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadInstancesWatcher,
                    enemyFactory)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}