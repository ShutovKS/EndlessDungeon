using System;
using System.Threading.Tasks;
using Abstract;
using Data.Addressable;
using Data.Settings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Infrastructure.GlobalStateMachine.States.MainMenu;
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
            ISaveLoadService saveLoadService, ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            PlayerStaticDefaultData playerStaticDefaultData, IPlayerFactory playerFactory)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _uiFactory = uiFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerStaticDefaultData = playerStaticDefaultData;
            _playerFactory = playerFactory;
        }

        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly PlayerStaticDefaultData _playerStaticDefaultData;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly IUIFactory _uiFactory;
        private GameObject _socketInstance;
        private GameObject _lootManagerInstance;

        public override async void Enter()
        {
            await CreateMap();
            await CreatePlayer();
            await CreatePlayerAddons();
            await CreateWeapons();
            await CreateMenu();
            CreateLootManager();
            await CreateSkillsBook();
            CreatePortal();

            Context.StateMachine.SwitchState<ProgressLoadingState, Type>(typeof(MainLocationState));
        }

        private async Task CreateMap()
        {
            var mainLocationMap =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_MAP);

            var mapInstance = _abstractFactory.CreateInstance(mainLocationMap, Vector3.zero);
        }

        private async Task CreatePlayer()
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            _playerFactory.CreatePlayer(player, _mainLocationSettings.PlayerSpawnPosition);
        }

        private async Task CreatePlayerAddons()
        {
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);

            _socketInstance = _abstractFactory.CreateInstance(
                socket,
                _mainLocationSettings.SocketForWeaponSpawnPosition);

            _socketInstance.transform.parent = _playerFactory.PlayerInstance.transform.GetChild(0).GetChild(0);
        }

        private async Task CreateWeapons()
        {
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMMER);

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
            weaponManagerInstance.AddComponent<WeaponManagerMainLocation>().SetUp(
                _socketInstance.transform,
                _playerStaticDefaultData,
                swordInstance,
                axInstance,
                hammerInstance);

            _saveLoadInstancesWatcher.RegisterProgress(weaponManagerInstance);
        }

        private async Task CreateMenu()
        {
            var playerInstance = _playerFactory.PlayerInstance;

            var menuInMainLocationScreen =
                await _assetsAddressableService.GetAsset<GameObject>(
                    AssetsAddressablesConstants.MENU_IN_MAIN_LOCATION_SCREEN);

            var menuInMainLocationScreenInstance =
                _abstractFactory.CreateInstance(menuInMainLocationScreen, Vector3.zero);

            menuInMainLocationScreenInstance.transform.SetParent(
                playerInstance.GetComponentInChildren<XRGazeInteractor>().transform.parent);

            menuInMainLocationScreenInstance.transform.localPosition = Vector3.zero;
            playerInstance.GetComponentInChildren<GazeInteractorTrigger>()
                .AddHoverEntered(_ => menuInMainLocationScreenInstance.SetActive(true));

            playerInstance.GetComponentInChildren<GazeInteractorTrigger>()
                .AddHoverExited(_ => menuInMainLocationScreenInstance.SetActive(false));

            menuInMainLocationScreenInstance.GetComponent<MenuInMainLocationScreen>().SetUp(
                _saveLoadService.SaveProgress,
                () => Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME,
                    typeof(MainMenuSetUpState)));

            menuInMainLocationScreenInstance.SetActive(false);
        }

        private void CreateLootManager()
        {
            _lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            _lootManagerInstance.AddComponent<LootManager>();
            _saveLoadInstancesWatcher.RegisterProgress(_lootManagerInstance);
        }

        private async Task CreateSkillsBook()
        {
            var skillsBookScreen =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SKILLS_BOOK_SCREEN);

            var skillsBookScreenInstance = _abstractFactory.CreateInstance(
                skillsBookScreen,
                _mainLocationSettings.SkillsBookScreenPosition);

            skillsBookScreenInstance.transform.rotation = Quaternion.Euler(
                _mainLocationSettings.SkillsBookScreenRotation.x,
                _mainLocationSettings.SkillsBookScreenRotation.y,
                _mainLocationSettings.SkillsBookScreenRotation.z);

            skillsBookScreenInstance.GetComponent<SkillsBook>().SetUp(_lootManagerInstance.GetComponent<LootManager>());

            _saveLoadInstancesWatcher.RegisterProgress(skillsBookScreenInstance);
        }

        private void CreatePortal()
        {
            var portalInstance = _abstractFactory.CreateInstance(
                new GameObject("Portal"),
                _mainLocationSettings.PortalSpawnPosition);

            var portalCollider = portalInstance.AddComponent<BoxCollider>();
            portalCollider.size = new Vector3(2.5f, 2.5f, 1f);
            portalCollider.isTrigger = true;
            portalInstance.AddComponent<Rigidbody>().isKinematic = true;
            portalInstance.AddComponent<PortalTrigger>().SetUp(
                () => Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME,
                    typeof(DungeonRoomGenerationState)));
        }
    }
}