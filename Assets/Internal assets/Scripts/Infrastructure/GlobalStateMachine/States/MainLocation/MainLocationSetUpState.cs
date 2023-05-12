﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Item.Weapon;
using Loot;
using Services.AssetsAddressableService;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using Skill;
using UI.MainLocation;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XR;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationSetUpState : State<GameInstance>
    {
        public MainLocationSetUpState(GameInstance context, IAbstractFactory abstractFactory, IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings,
            ISaveLoadService saveLoadService, ISaveLoadInstancesWatcher saveLoadInstancesWatcher)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _uiFactory = uiFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        public override async void Enter()
        {
            var mainLocationMap = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_MAP);
            var portal = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PORTAL);
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var socket = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);
            var menuInMainLocationScreen = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MENU_IN_MAIN_LOCATION_SCREEN);
            var skillsBookScreen = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SKILLS_BOOK_SCREEN);
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMMER);

            var mapInstance = _abstractFactory.CreateInstance(mainLocationMap, Vector3.zero);

            var playerInstance = _abstractFactory.CreateInstance(player, _mainLocationSettings.PlayerSpawnPosition);

            var socketInstance = _abstractFactory.CreateInstance(socket, _mainLocationSettings.SocketForWeaponSpawnPosition);
            socketInstance.transform.parent = playerInstance.transform.GetChild(0).GetChild(0);
            
            var portalInstance = _abstractFactory.CreateInstance(portal, _mainLocationSettings.PortalSpawnPosition);
            portalInstance.transform.rotation = new Quaternion(
                _mainLocationSettings.PortalSpawnRotation.x,
                _mainLocationSettings.PortalSpawnRotation.y,
                _mainLocationSettings.PortalSpawnRotation.z,
                0);
            if (portalInstance.transform.GetChild(0).TryGetComponent<XRGrabInteractable>(out var xrGrabInteractable))
                xrGrabInteractable.selectEntered.AddListener(_ => Context.StateMachine.SwitchState<DungeonRoomLoadingState>());
            
            var swordInstance = _abstractFactory.CreateInstance(
                sword,
                _mainLocationSettings.WeaponSpawnPosition[(int)sword.GetComponent<Weapon>().WeaponType]);

            var axInstance = _abstractFactory.CreateInstance(
                ax,
                _mainLocationSettings.WeaponSpawnPosition[(int)ax.GetComponent<Weapon>().WeaponType]);

            var hammerInstance = _abstractFactory.CreateInstance(
                hammer,
                _mainLocationSettings.WeaponSpawnPosition[(int)hammer.GetComponent<Weapon>().WeaponType]);

            var weaponManagerInstance = _abstractFactory.CreateInstance(new GameObject("weaponManager"), Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManagerMainLocation>().SetUp(socketInstance, swordInstance, axInstance, hammerInstance);

            var lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            lootManagerInstance.AddComponent<LootManager>();

            var menuInMainLocationScreenInstance = _abstractFactory.CreateInstance(menuInMainLocationScreen, Vector3.zero);
            menuInMainLocationScreenInstance.transform.SetParent(playerInstance.GetComponentInChildren<XRGazeInteractor>().transform.parent);
            menuInMainLocationScreenInstance.transform.localPosition = Vector3.zero;
            playerInstance.GetComponentInChildren<GazeInteractorTrigger>().AddHoverEntered(_ => menuInMainLocationScreenInstance.SetActive(true));
            playerInstance.GetComponentInChildren<GazeInteractorTrigger>().AddHoverExited(_ => menuInMainLocationScreenInstance.SetActive(false));
            menuInMainLocationScreenInstance.GetComponent<MenuInMainLocationScreen>().SetUp(_saveLoadService);
            menuInMainLocationScreenInstance.SetActive(false);

            var skillsBookScreenInstance = _abstractFactory.CreateInstance(skillsBookScreen, _mainLocationSettings.SkillsBookScreenPosition);
            skillsBookScreenInstance.transform.rotation = Quaternion.Euler(
                _mainLocationSettings.SkillsBookScreenRotation.x, 
                _mainLocationSettings.SkillsBookScreenRotation.y, 
                _mainLocationSettings.SkillsBookScreenRotation.z);
            skillsBookScreenInstance.GetComponent<SkillsBook>().SetUp(lootManagerInstance.GetComponent<LootManager>());

            _saveLoadInstancesWatcher.RegisterProgress(
                weaponManagerInstance,
                lootManagerInstance, 
                skillsBookScreenInstance);
            
            Context.StateMachine.SwitchState<ProgressLoadingForMainState>();
        }
    }
}