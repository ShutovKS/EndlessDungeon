using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
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
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, IAbstractFactory abstractFactory,
            IPlayerFactory playerFactory) :
            base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
        }

        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;

        public override async void Enter()
        {
            await _uiFactory.CreateMainLocationScreen();

            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _playerFactory.DestroyPlayer();
            _saveLoadService.SaveProgress();
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMainLocationScreen();
            _saveLoadInstancesWatcher.ClearProgress();
        }
    }
}