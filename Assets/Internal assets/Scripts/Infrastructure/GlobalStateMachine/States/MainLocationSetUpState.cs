using System.Collections.Generic;
using Data.Addressable;
using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Item.Weapon;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationSetUpState : State<GameInstance>
    {
        public MainLocationSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings,
            IPersistentProgressService persistentProgressService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _persistentProgressService = persistentProgressService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        public override async void Enter()
        {
            var mainLocationMap =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_MAP);
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMER);
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);


            var mapInstance = _abstractFactory.CreateInstance(mainLocationMap, Vector3.zero);
            var playerInstance = _abstractFactory.CreateInstance(player, _mainLocationSettings.PlayerSpawnPosition);

            var swordInstance = _abstractFactory.CreateInstance(sword,
                _mainLocationSettings.WeaponSpawnPosition[(int)sword.GetComponent<Weapon>().WeaponType]);
            var axInstance = _abstractFactory.CreateInstance(ax,
                _mainLocationSettings.WeaponSpawnPosition[(int)ax.GetComponent<Weapon>().WeaponType]);
            var hammerInstance = _abstractFactory.CreateInstance(hammer,
                _mainLocationSettings.WeaponSpawnPosition[(int)hammer.GetComponent<Weapon>().WeaponType]);

            var socketInstance =
                _abstractFactory.CreateInstance(socket, _mainLocationSettings.SocketForWeaponSpawnPosition);
            socketInstance.transform.parent = playerInstance.transform.GetChild(0).GetChild(0);

            var weaponManagerInstance = _abstractFactory.CreateInstance(socket, Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManager>()
                .SetUp(socket, swordInstance, axInstance, hammerInstance);

            _saveLoadInstancesWatcher.RegisterProgress(weaponManagerInstance);

            Context.StateMachine.SwitchState<ProgressLoadingState, GameObject>(playerInstance);
        }
    }
}