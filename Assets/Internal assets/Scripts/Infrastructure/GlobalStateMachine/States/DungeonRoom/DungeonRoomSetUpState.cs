using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Settings;
using Data.Static;
using DungeonGenerator;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Item.Weapon;
using Loot;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using Units.Enemy;
using Units.Player;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpState : StateOneParam<GameInstance, (DungeonTilesType[,] dungeonMap, (int, int)
        playerPosition, List<(int, int)> enemiesPosition)>
    {
        public DungeonRoomSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, DungeonRoomSettings dungeonRoomSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, PlayerStaticDefaultData playerStaticDefaultData,
            IEnemyFactory enemyFactory, IPlayerFactory playerFactory)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _dungeonRoomSettings = dungeonRoomSettings;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerStaticDefaultData = playerStaticDefaultData;
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly DungeonRoomSettings _dungeonRoomSettings;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly PlayerStaticDefaultData _playerStaticDefaultData;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IPlayerFactory _playerFactory;
        private Transform _socketInstanceTransform;


        private const float UNIT = 4.85f / 2;

        public override async void Enter(
            (DungeonTilesType[,] dungeonMap, (int, int) playerPosition, List<(int, int)> enemiesPosition)
                dungeonArchitecture)
        {
            await CreateMap(dungeonArchitecture.dungeonMap);
            await CreatePlayer(dungeonArchitecture.playerPosition);
            await CreatePlayerAddons();
            await CreateWeapon();
            CreateLootManager();
            BuildNavMeshForDungeon();
            await CreateEnemies(dungeonArchitecture.enemiesPosition);

            Context.StateMachine.SwitchState<ProgressLoadingState, Type>(typeof(DungeonRoomState));
        }

        private async Task CreateMap(DungeonTilesType[,] dungeonMap)
        {
            var floor = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.FLOOR);
            var wall = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WALL);


            for (var y = 0; y < dungeonMap.GetLength(0); y++)
            for (var x = 0; x < dungeonMap.GetLength(1); x++)
            {
                switch (dungeonMap[y, x])
                {
                    case DungeonTilesType.FLOOR:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        break;
                    case DungeonTilesType.WALL:
                        _abstractFactory.CreateInstance(wall, new Vector3(x, 0, y) * UNIT);
                        break;
                }
            }
        }

        private async Task CreatePlayer((int x, int y) playerPosition)
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);

            _playerFactory.CreatePlayer(player, new Vector3(playerPosition.x, 0, playerPosition.y) * UNIT);

            _playerFactory.PlayerInstance.AddComponent<Player>().SetUp(
                () => Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                    AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME,
                    typeof(MainLocationSetUpState)),
                _playerStaticDefaultData);

            _saveLoadInstancesWatcher.RegisterProgress(_playerFactory.PlayerInstance);
        }

        private async Task CreatePlayerAddons()
        {
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);

            var socketInstance = _abstractFactory.CreateInstance(socket, Vector3.zero);
            socketInstance.transform.parent = _playerFactory.PlayerInstance.transform.GetChild(0).GetChild(0);
            socketInstance.transform.localPosition = _dungeonRoomSettings.SocketForWeaponSpawnPosition;
            _socketInstanceTransform = socketInstance.transform;

            var enemyDetectorInstance = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero);
            enemyDetectorInstance.transform.parent = _playerFactory.PlayerInstance.transform;
            enemyDetectorInstance.transform.localPosition = Vector3.zero;
            enemyDetectorInstance.AddComponent<EnemyDetector>();
            enemyDetectorInstance.AddComponent<SphereCollider>().radius = 7.5f;
            enemyDetectorInstance.GetComponent<SphereCollider>().isTrigger = true;
        }

        private async Task CreateWeapon()
        {
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMMER);

            var swordInstance = _abstractFactory.CreateInstance(sword, Vector3.zero);
            var axInstance = _abstractFactory.CreateInstance(ax, Vector3.zero);
            var hammerInstance = _abstractFactory.CreateInstance(hammer, Vector3.zero);

            var weaponManagerInstance = _abstractFactory.CreateInstance(new GameObject("weaponManager"), Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManagerDungeonRoom>().SetUp(
                _socketInstanceTransform,
                _playerStaticDefaultData,
                swordInstance,
                axInstance,
                hammerInstance);

            _saveLoadInstancesWatcher.RegisterProgress(weaponManagerInstance);
        }

        private void CreateLootManager()
        {
            var lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            lootManagerInstance.AddComponent<LootManager>();

            _saveLoadInstancesWatcher.RegisterProgress(lootManagerInstance);
        }

        private void BuildNavMeshForDungeon()
        {
            var navMeshSurface = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero)
                .AddComponent<NavMeshSurface>();

            navMeshSurface.BuildNavMesh();
        }

        private async Task CreateEnemies(List<(int, int)> enemyPosition)
        {
            foreach (var position in enemyPosition)
            {
                var enemy = await _enemyFactory.CreateInstance(
                    (EnemyType)Random.Range(0, 5),
                    new Vector3(position.Item1 * UNIT, 2, position.Item2 * UNIT));

                enemy.transform.rotation = new Quaternion(0, Random.Range(-180f, 180f), 0, 0);
            }
        }
    }
}