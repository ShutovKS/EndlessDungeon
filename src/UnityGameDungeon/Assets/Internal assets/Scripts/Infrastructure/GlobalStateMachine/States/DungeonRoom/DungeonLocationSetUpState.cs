#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Addressable;
using Data.LocationSettings;
using Data.Static;
using DungeonGenerator.Tiles;
using DungeonGenerator.Tiles.Interface;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Item.WeaponManager;
using Loot;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using Units.Enemy;
using Units.Player;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using XR;
using Random = UnityEngine.Random;

#endregion

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonLocationSetUpState : StateWithParam<GameInstance, (ITile[,] dungeonMap, (int x, int y)
        playerPosition, List<(int x, int y)>enemiesPosition)>
    {
        public DungeonLocationSetUpState(GameInstance context,
            IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService,
            DungeonRoomSettings dungeonRoomSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            PlayerStaticDefaultData playerStaticDefaultData,
            IEnemyFactory enemyFactory,
            IPlayerFactory playerFactory,
            IUIFactory uiFactory)
            : base(context)
        {
            _assetsAddressableService = assetsAddressableService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerStaticDefaultData = playerStaticDefaultData;
            _dungeonRoomSettings = dungeonRoomSettings;
            _abstractFactory = abstractFactory;
            _playerFactory = playerFactory;
            _enemyFactory = enemyFactory;
            _uiFactory = uiFactory;
        }

        private const float UNIT = 4.85f / 2;
        private readonly IAbstractFactory _abstractFactory;

        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly DungeonRoomSettings _dungeonRoomSettings;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly PlayerStaticDefaultData _playerStaticDefaultData;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IUIFactory _uiFactory;
        private Transform _socketInstanceTransform;

        public override async void Enter(
            (ITile[,] dungeonMap, (int x, int y) playerPosition, List<(int x, int y)> enemiesPosition)
                dungeonArchitecture)
        {
            await CreateMap(dungeonArchitecture.dungeonMap);
            await LightBake();
            await BuildNavMeshForDungeon();
            await CreatePlayer(dungeonArchitecture.playerPosition);
            await CreatePlayerAddons();
            await CreateWeapon();
            await CreateMenu();
            await CreateLootManager();
            await CreateEnemies(dungeonArchitecture.enemiesPosition);

            Context.StateMachine.SwitchState(typeof(ProgressLoadingState), typeof(DungeonLocationState));
        }

        private async Task CreateMap(ITile[,] dungeonMap)
        {
            var floor = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.FLOOR);
            var wall = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WALL);
            var wallAndLight =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WALL_AND_LIGHT);


            for (var y = 0; y < dungeonMap.GetLength(0); y++)
            for (var x = 0; x < dungeonMap.GetLength(1); x++)
            {
                switch (dungeonMap[y, x])
                {
                    case FloorTile:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 2, y) * UNIT).transform.rotation =
                            new Quaternion(180, 0, 0, 0);

                        break;
                    case WallTile tile:
                        if (tile.IsLight)
                        {
                            _abstractFactory.CreateInstance(wallAndLight, new Vector3(x, 0, y) * UNIT).transform
                                .rotation = Quaternion.Euler(0, 90 * (int)tile.LightDirectionType, 0);
                        }
                        else
                        {
                            _abstractFactory.CreateInstance(wall, new Vector3(x, 0, y) * UNIT);
                        }

                        break;
                }
            }
        }

        private async Task LightBake()
        {
            // Lightmapping.BakeAsync(SceneManager.GetActiveScene());
        }
        
        private async Task BuildNavMeshForDungeon()
        {
            var navMeshSurface = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero)
                .AddComponent<NavMeshSurface>();

            navMeshSurface.BuildNavMesh();
        }

        private async Task CreatePlayer((int x, int y) playerPosition)
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.PLAYER);

            _playerFactory.CreatePlayer(player, new Vector3(playerPosition.x, 0, playerPosition.y) * UNIT);

            _playerFactory.PlayerInstance.AddComponent<Player>().SetUp(_playerStaticDefaultData.HealthMaxPoints, _playerStaticDefaultData.ProtectionPoints);
            _playerFactory.PlayerInstance.GetComponent<Player>().RegisterOnPlayerDead(MoveMainLocation);
            _saveLoadInstancesWatcher.RegisterProgressWatchers(_playerFactory.PlayerInstance);

            void MoveMainLocation()
            {
                Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                    typeof(SceneLoadingState),
                    (AssetsAddressableConstants.MAIN_LOCATION_SCENE_NAME, typeof(MainLocationSetUpState)));
            }
        }

        private async Task CreatePlayerAddons()
        {
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.SOCKET_FOR_SWORD);

            var socketInstance = _abstractFactory.CreateInstance(socket, Vector3.zero);
            socketInstance.transform.parent = _playerFactory.PlayerInstance.transform.GetChild(0).GetChild(0);
            socketInstance.transform.localPosition = _dungeonRoomSettings.SocketForWeaponSpawnPosition;
            _socketInstanceTransform = socketInstance.transform;

            var enemyDetectorInstance = _abstractFactory.CreateInstance(new GameObject("EnemyDetector"), Vector3.zero);
            enemyDetectorInstance.transform.parent = _playerFactory.PlayerInstance.transform;
            enemyDetectorInstance.transform.localPosition = Vector3.zero;
            enemyDetectorInstance.AddComponent<EnemyDetector>();
            enemyDetectorInstance.AddComponent<SphereCollider>().radius = 12.5f;
            enemyDetectorInstance.GetComponent<SphereCollider>().isTrigger = true;
        }

        private async Task CreateWeapon()
        {
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_AX);
            var hammer =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressableConstants.WEAPON_HAMMER);

            var swordInstance = _abstractFactory.CreateInstance(sword, Vector3.zero);
            var axInstance = _abstractFactory.CreateInstance(ax, Vector3.zero);
            var hammerInstance = _abstractFactory.CreateInstance(hammer, Vector3.zero);

            var weaponManagerInstance = _abstractFactory.CreateInstance(new GameObject("weaponManager"), Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManagerDungeonRoom>().SetUp(
                _socketInstanceTransform,
                _playerStaticDefaultData.DamagePoints,
                swordInstance,
                axInstance,
                hammerInstance);

            _saveLoadInstancesWatcher.RegisterProgressWatchers(weaponManagerInstance);
        }

        private async Task CreateMenu()
        {
            var playerInstance = _playerFactory.PlayerInstance;

            var menuInDungeonRoomScreenInstance = await _uiFactory.CreateMenuInDungeonRoomScreen();

            menuInDungeonRoomScreenInstance.transform.SetParent(
                playerInstance.GetComponentInChildren<XRGazeInteractor>().transform.parent);

            menuInDungeonRoomScreenInstance.transform.localPosition = Vector3.zero;

            playerInstance.GetComponentInChildren<GazeInteractTrigger>()
                .AddHoverEntered(_ => menuInDungeonRoomScreenInstance.SetActive(true));

            playerInstance.GetComponentInChildren<GazeInteractTrigger>()
                .AddHoverExited(_ => menuInDungeonRoomScreenInstance.SetActive(false));

            menuInDungeonRoomScreenInstance.SetActive(false);
        }

        private async Task CreateLootManager()
        {
            var lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            lootManagerInstance.AddComponent<LootManager>();

            _saveLoadInstancesWatcher.RegisterProgressWatchers(lootManagerInstance);
        }

        private async Task CreateEnemies(List<(int x, int y)> enemyPosition)
        {
            foreach (var position in enemyPosition)
            {
                var enemy = await _enemyFactory.CreateInstance(
                    (EnemyType)Random.Range(0, 6),
                    new Vector3(position.x * UNIT, 2, position.y * UNIT));

                enemy.transform.rotation = new Quaternion(0, Random.Range(-180f, 180f), 0, 0);
            }
        }
    }
}
