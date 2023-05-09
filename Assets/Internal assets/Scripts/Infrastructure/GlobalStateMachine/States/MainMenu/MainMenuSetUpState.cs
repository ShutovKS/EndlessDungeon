using Data.Addressable;
using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuSetUpState : State<GameInstance>
    {
        public MainMenuSetUpState(GameInstance context, IAbstractFactory abstractFactory, IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService, MainMenuSettings mainMenuSettings) : base(context)
        {
            _abstractFactory = abstractFactory;
            _uiFactory = uiFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainMenuSettings = mainMenuSettings;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainMenuSettings _mainMenuSettings;

        public override async void Enter()
        {
            _uiFactory.DestroyLoadingScreen();

            var map = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_MENU_MAP);
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);

            var mapInstance = _abstractFactory.CreateInstance(map, Vector3.zero);
            var playerInstance = _abstractFactory.CreateInstance(player, _mainMenuSettings.PlayerSpawnPosition);
            
            var mainMenuScreenInstance = await _uiFactory.CreateMainMenuScreen();
            mainMenuScreenInstance.transform.position = _mainMenuSettings.UIMenuSpawnPosition;
            
            Context.StateMachine.SwitchState<MainMenuState, GameObject>(mainMenuScreenInstance);
        }
    }
}