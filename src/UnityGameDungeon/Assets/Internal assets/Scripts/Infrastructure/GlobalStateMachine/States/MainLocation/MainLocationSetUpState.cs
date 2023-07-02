#region

using System;
using System.Threading.Tasks;
using Data.Addressable;
using Data.LocationSettings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Item.Weapon;
using Item.WeaponManager;
using Loot;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using Skill;
using TransitionToTheDungeon;
using UnityEngine;
using UnityEditor.Experimental;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using XR;

#endregion

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationSetUpState : State<GameInstance>
    {
        public MainLocationSetUpState(
            GameInstance context,
            IAbstractFactory abstractFactory,
            IUIFactory uiFactory,
            IAssetsAddressableService assetsAddressableService,
            MainLocationSettings mainLocationSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            PlayerStaticDefaultData playerStaticDefaultData,
            IPlayerFactory playerFactory) : base(context)
        {
            _assetsAddressableService = assetsAddressableService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerStaticDefaultData = playerStaticDefaultData;
            _mainLocationSettings = mainLocationSettings;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
            _uiFactory = uiFactory;
        }

        private readonly IAbstractFactory _abstractFactory;

        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly IPlayerFactory _playerFactory;
        private readonly PlayerStaticDefaultData _playerStaticDefaultData;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IUIFactory _uiFactory;

        private GameObject _lootManagerInstance;
        private GameObject _socketInstance;

        public override async void Enter()
        {
            CreateLootManager();
            await CreateMap();
            await LightBake();
            await CreatePlayer();
            await CreatePlayerAddons();
            await CreateSkillsBookScreen();
            await CreateWeapons();
            await CreateMenu();
            CreateTransitionToTheDungeon();

            Context.StateMachine.SwitchState(typeof(ProgressLoadingState), typeof(MainLocationState));
        }

        private async Task CreateMap()
        {
            var mainLocationMap =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.MAIN_LOCATION_MAP);

            _abstractFactory.CreateInstance(mainLocationMap, Vector3.zero);
        }

        private async Task LightBake()
        {
            // Lightmapping.BakeAsync(SceneManager.GetActiveScene());
        }

        private async Task CreatePlayer()
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.PLAYER);
            _playerFactory.CreatePlayer(player, _mainLocationSettings.PlayerSpawnPosition);
        }

        private async Task CreatePlayerAddons()
        {
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.SOCKET_FOR_SWORD);

            _socketInstance = _abstractFactory.CreateInstance(socket, Vector3.zero);
            _socketInstance.transform.parent = _playerFactory.PlayerInstance.transform.GetChild(0).GetChild(0);
            _socketInstance.transform.localPosition = _mainLocationSettings.SocketForWeaponSpawnPosition;
        }

        private void CreateLootManager()
        {
            _lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            _lootManagerInstance.AddComponent<LootManager>();

            _saveLoadInstancesWatcher.RegisterProgressWatchers(_lootManagerInstance);
        }

        private async Task CreateSkillsBookScreen()
        {
            var skillsBookScreenInstance = await _uiFactory.CreateSkillsBookScreen();

            skillsBookScreenInstance.transform.position = _mainLocationSettings.SkillsBookScreenPosition;

            skillsBookScreenInstance.transform.rotation =
                Quaternion.Euler(_mainLocationSettings.SkillsBookScreenRotation);

            var lootManager = _lootManagerInstance.GetComponent<LootManager>();
            skillsBookScreenInstance.GetComponent<SkillsManager>().SetUp(
                lootManager.TryAmountChangeOnThe,
                () => lootManager.SoulsOfTheDungeon);

            _saveLoadInstancesWatcher.RegisterProgressWatchers(skillsBookScreenInstance);
        }

        private async Task CreateWeapons()
        {
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_AX);
            var hammer = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_HAMMER);

            var axInstance = _abstractFactory.CreateInstance(
                ax,
                _mainLocationSettings.WeaponSpawnPosition[(int)ax.GetComponent<Weapon>().WeaponType]);

            var hammerInstance = _abstractFactory.CreateInstance(
                hammer,
                _mainLocationSettings.WeaponSpawnPosition[(int)hammer.GetComponent<Weapon>().WeaponType]);

            var swordInstance = _abstractFactory.CreateInstance(
                sword,
                _mainLocationSettings.WeaponSpawnPosition[(int)sword.GetComponent<Weapon>().WeaponType]);

            var weaponManagerInstance = _abstractFactory.CreateInstance(new GameObject("weaponManager"), Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManagerMainLocation>().SetUp(
                _socketInstance.transform,
                _playerStaticDefaultData.DamagePoints,
                axInstance,
                hammerInstance,
                swordInstance);

            _saveLoadInstancesWatcher.RegisterProgressWatchers(weaponManagerInstance);
        }

        private async Task CreateMenu()
        {
            var playerInstance = _playerFactory.PlayerInstance;

            var menuInMainLocationScreenInstance = await _uiFactory.CreateMenuInMainLocationScreen();

            menuInMainLocationScreenInstance.transform.SetParent(
                playerInstance.GetComponentInChildren<XRGazeInteractor>().transform.parent);

            menuInMainLocationScreenInstance.transform.localPosition = Vector3.zero;

            playerInstance.GetComponentInChildren<GazeInteractTrigger>()
                .AddHoverEntered(_ => menuInMainLocationScreenInstance.SetActive(true));

            playerInstance.GetComponentInChildren<GazeInteractTrigger>()
                .AddHoverExited(_ => menuInMainLocationScreenInstance.SetActive(false));

            menuInMainLocationScreenInstance.SetActive(false);
        }

        private void CreateTransitionToTheDungeon()
        {
            var portalInstance = _abstractFactory.CreateInstance(
                new GameObject("Transition to the Dungeon"),
                _mainLocationSettings.TransitionToTheDungeonSpawnPosition);

            var portalCollider = portalInstance.AddComponent<BoxCollider>();
            portalCollider.size = new Vector3(2.5f, 2.5f, 1f);
            portalCollider.isTrigger = true;

            portalInstance.AddComponent<Rigidbody>().isKinematic = true;
            portalInstance.AddComponent<TransitionToTheDungeonTrigger>()
                .RegisterOnTransitionToTheDungeon(MoveToDungeonRoom);

            void MoveToDungeonRoom()
            {
                Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                    typeof(SceneLoadingState),
                    (AssetsAddressableConstants.DUNGEON_ROOM_SCENE_NAME, typeof(DungeonLocationGenerationState)));
            }
        }
    }
}