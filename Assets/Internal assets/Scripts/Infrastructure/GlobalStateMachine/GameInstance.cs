using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States;
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
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService)
        {
            StateMachine = new StateMachine<GameInstance>(this,
                new BootstrapState(this),
                new MainMenuState(this, uiFactory),
                
                new SceneLoadingForMainLocationState(this, uiFactory),
                new MainLocationLoadingState(this, uiFactory),
                new MainLocationSetUpState(this, abstractFactory,
                    assetsAddressableService, mainLocationSettings,
                    saveLoadInstancesWatcher),
                new ProgressLoadingForMainState(this, saveLoadService,
                    saveLoadInstancesWatcher, persistentProgressService),
                new MainLocationState(this, uiFactory,
                    saveLoadService, abstractFactory),
                
                new SceneLoadingForDungeonRoomState(this, uiFactory),
                new DungeonRoomLoadingState(this, uiFactory),
                new DungeonRoomGenerationState(this),
                new DungeonRoomSetUpState(this, abstractFactory, assetsAddressableService,
                    mainLocationSettings, saveLoadInstancesWatcher),
                new DungeonRoomState(this, uiFactory)
            );
            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}