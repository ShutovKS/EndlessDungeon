using Data.Settings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States;
using Infrastructure.GlobalStateMachine.States.MainMenu;
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
            MainMenuSettings mainMenuSettings,
            PlayerStaticDefaultData playerStaticDefaultData,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService,
            IEnemyFactory enemyFactory)
        {
            StateMachine = new StateMachine<GameInstance>(
                this,
                new BootstrapState(this),
                new MainMenuLoadingState(this, uiFactory),
                new MainMenuSetUpState(this, abstractFactory, uiFactory, assetsAddressableService, mainMenuSettings),
                new MainMenuState(this, uiFactory, abstractFactory),
                new MainLocationLoadingState(this, uiFactory),
                new MainLocationSetUpState(
                    this,
                    abstractFactory,
                    uiFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData),
                new ProgressLoadingForMainState(
                    this,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    persistentProgressService),
                new MainLocationState(
                    this,
                    uiFactory,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    abstractFactory),
                new DungeonRoomLoadingState(this, uiFactory),
                new DungeonRoomGenerationState(this),
                new DungeonRoomSetUpState(
                    this,
                    abstractFactory,
                    assetsAddressableService,
                    mainLocationSettings,
                    saveLoadInstancesWatcher,
                    playerStaticDefaultData),
                new DungeonRoomSetUpNavMeshState(this, abstractFactory),
                new DungeonRoomSetUpEnemyState(this, enemyFactory),
                new ProgressLoadingForDungeonRoom(
                    this,
                    saveLoadService,
                    saveLoadInstancesWatcher,
                    persistentProgressService),
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