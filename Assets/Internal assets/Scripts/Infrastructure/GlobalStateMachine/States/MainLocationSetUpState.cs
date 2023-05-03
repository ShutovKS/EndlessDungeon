using Data.Addressable;
using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using UI.MainMenu;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationSetUpState : StateOneParam<GameInstance, MainMenuScreen>
    {
        public MainLocationSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;

        public override async void Enter(MainMenuScreen mainMenuScreen)
        {
            var mainLocationMap =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_MAP);
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMER);

            var mapInstance = _abstractFactory.CreateInstance(mainLocationMap, Vector3.zero);
            var playerInstance = _abstractFactory.CreateInstance(player, _mainLocationSettings.PlayerSpawnPosition);
            var swordInstance = _abstractFactory.CreateInstance(sword, _mainLocationSettings.WeaponSpawnPosition[0]);
            var axInstance = _abstractFactory.CreateInstance(ax, _mainLocationSettings.WeaponSpawnPosition[1]);
            var hammerInstance = _abstractFactory.CreateInstance(hammer, _mainLocationSettings.WeaponSpawnPosition[2]);

            Context.StateMachine.SwitchState<ProgressLoadingState, GameObject>(playerInstance);
        }
    }
}