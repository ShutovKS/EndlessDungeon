using System.Threading;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuSetUpState : State<GameInstance>
    {
        public MainMenuSetUpState(GameInstance context, IAbstractFactory abstractFactory, IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService, MainMenuSettings mainMenuSettings,
            IPlayerFactory playerFactory) : base(context)
        {
            _assetsAddressableService = assetsAddressableService;
            _mainMenuSettings = mainMenuSettings;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainMenuSettings _mainMenuSettings;
        private readonly IAbstractFactory _abstractFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;
        private GameObject _mainMenuScreenInstance;

        public override async void Enter()
        {
            _uiFactory.DestroyLoadingScreen();

            await CreateMap();
            await CreatePlayer();
            await CreateMainMenu();

            Context.StateMachine.SwitchState<MainMenuState, GameObject>(_mainMenuScreenInstance);
        }

        private async Task CreatePlayer()
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            _playerFactory.CreatePlayer(player, _mainMenuSettings.PlayerSpawnPosition);
        }

        private async Task CreateMap()
        {
            var map = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_MENU_MAP);
            _abstractFactory.CreateInstance(map, Vector3.zero);
        }

        private async Task CreateMainMenu()
        {
            var mainMenuScreen =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_MENU_SCREEN);

            _mainMenuScreenInstance = _abstractFactory.CreateInstance(
                mainMenuScreen,
                _mainMenuSettings.UIMenuSpawnPosition);
        }
    }
}