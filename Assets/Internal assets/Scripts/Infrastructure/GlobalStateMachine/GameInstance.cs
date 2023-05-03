using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States;
using Services.AssetsAddressableService;

namespace Infrastructure.GlobalStateMachine
{
    public class GameInstance
    {
        public GameInstance(IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService,
            IAbstractFactory abstractFactory,
            MainLocationSettings mainLocationSettings)
        {
            StateMachine = new StateMachine<GameInstance>(this, new BootstrapState(this),
                new SceneLoadingState(this, uiFactory),
                new MainMenuState(this, uiFactory),
                new MainLocationLoadingState(this, uiFactory),
                new MainLocationSetUpState(this, abstractFactory, assetsAddressableService, mainLocationSettings),
                new ProgressLoadingState(this), //TODO added load progress
                new MainLocationGameplayState(this, uiFactory)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public readonly StateMachine<GameInstance> StateMachine;
    }
}