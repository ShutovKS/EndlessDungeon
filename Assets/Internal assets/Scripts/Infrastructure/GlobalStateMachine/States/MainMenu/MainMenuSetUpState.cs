#region

using System.Threading.Tasks;
using Data.Addressable;
using Data.LocationSettings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using UnityEngine;

#endregion

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuSetUpState : State<GameInstance>
    {
        public MainMenuSetUpState(
            GameInstance context,
            IAbstractFactory abstractFactory,
            IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService,
            MainMenuSettings mainMenuSettings,
            IPlayerFactory playerFactory) : base(context)
        {
            _assetsAddressableService = assetsAddressableService;
            _mainMenuSettings = mainMenuSettings;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAbstractFactory _abstractFactory;

        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainMenuSettings _mainMenuSettings;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;

        public override async void Enter()
        {
            _uiFactory.DestroyLoadingScreen();

            await CreateMap();
            await CreatePlayer();
            await CreateMainMenu();

            Context.StateMachine.SwitchState(typeof(MainMenuState));
        }

        private async Task CreateMap()
        {
            var map = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.MAIN_MENU_MAP);
            _abstractFactory.CreateInstance(map, Vector3.zero);
        }

        private async Task CreatePlayer()
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.PLAYER);
            _playerFactory.CreatePlayer(player, _mainMenuSettings.PlayerSpawnPosition);
        }

        private async Task CreateMainMenu()
        {
            var menu = await _uiFactory.CreateMainMenuScreen();
            menu.transform.position = _mainMenuSettings.UIMenuSpawnPosition;
        }
    }
}
