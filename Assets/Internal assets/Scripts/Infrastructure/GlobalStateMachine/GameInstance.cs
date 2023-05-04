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
                new SceneLoadingState(this, uiFactory),
                new MainMenuState(this, uiFactory),
                new MainLocationLoadingState(this, uiFactory),
                new ProgressLoadingState(this, saveLoadService,
                    saveLoadInstancesWatcher, 
                    persistentProgressService),
                new MainLocationSetUpState(this, abstractFactory, 
                    assetsAddressableService,
                    mainLocationSettings,
                    persistentProgressService,
                    saveLoadInstancesWatcher),
                new MainLocationGameplayState(this, uiFactory,
                    persistentProgressService, 
                    saveLoadService)
            );
            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}