using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UI.MainLocation;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationState : State<GameInstance>
    {
        public MainLocationState(GameInstance context, IUIFactory uiFactory, ISaveLoadService saveLoadService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, IAbstractFactory abstractFactory) :
            base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _abstractFactory = abstractFactory;
        }

        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IAbstractFactory _abstractFactory;

        public override async void Enter()
        {
            await _uiFactory.CreateMainLocationScreen();

            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _saveLoadService.SaveProgress();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMainLocationScreen();
            _saveLoadInstancesWatcher.ClearProgress();
        }
    }
}