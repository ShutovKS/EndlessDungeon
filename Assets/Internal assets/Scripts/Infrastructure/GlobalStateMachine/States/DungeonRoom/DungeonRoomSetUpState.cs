using Data.Addressable;
using Data.Settings;
using Data.Static;
using GeneratorDungeons;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Item.Weapon;
using Loot;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using Units.Player;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpState : StateOneParam<GameInstance, MapDungeon>
    {
        public DungeonRoomSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, PlayerStaticDefaultData playerStaticDefaultData)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _playerStaticDefaultData = playerStaticDefaultData;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly PlayerStaticDefaultData _playerStaticDefaultData;

        private const float UNIT = 4.85f / 2;

        public override async void Enter(MapDungeon mapDungeon)
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var socket = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);
            var floor = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.FLOOR);
            var wall = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WALL);
            var sword = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_SWORD);
            var ax = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_AX);
            var hammer = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WEAPON_HAMMER);

            var playerInstance = _abstractFactory.CreateInstance(player, Vector3.zero);
            playerInstance.AddComponent<Player>().SetUp(Context.StateMachine.SwitchState<MainLocationLoadingState>, _playerStaticDefaultData);
            var socketInstance = _abstractFactory.CreateInstance(socket, _mainLocationSettings.SocketForWeaponSpawnPosition);
            socketInstance.transform.parent = playerInstance.transform.GetChild(0).GetChild(0);
            
            var swordInstance = _abstractFactory.CreateInstance(sword, Vector3.zero);
            var axInstance = _abstractFactory.CreateInstance(ax, Vector3.zero);
            var hammerInstance = _abstractFactory.CreateInstance(hammer, Vector3.zero);
            var weaponManagerInstance = _abstractFactory.CreateInstance(new GameObject("weaponManager"), Vector3.zero);
            weaponManagerInstance.AddComponent<WeaponManagerDungeonRoom>().SetUp(socketInstance, _playerStaticDefaultData, swordInstance, axInstance, hammerInstance);

            var enemyDetectorInstance = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero);
            enemyDetectorInstance.transform.parent = playerInstance.transform;
            enemyDetectorInstance.AddComponent<EnemyDetector>();
            enemyDetectorInstance.AddComponent<SphereCollider>().radius = 7.5f;
            enemyDetectorInstance.GetComponent<SphereCollider>().isTrigger = true;

            var lootManagerInstance = _abstractFactory.CreateInstance(new GameObject("lootManager"), Vector3.zero);
            lootManagerInstance.AddComponent<LootManager>();

            for (var y = 0; y < mapDungeon.TilesMap.GetLength(0); y++)
            for (var x = 0; x < mapDungeon.TilesMap.GetLength(1); x++)
            {
                switch (mapDungeon.TilesMap[y, x])
                {
                    case Tiles.FLOOR:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        break;
                    case Tiles.WALL:
                        _abstractFactory.CreateInstance(wall, new Vector3(x, 0, y) * UNIT);
                        break;
                    case Tiles.PLAYER:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        playerInstance.transform.position = new Vector3(x, 0, y) * UNIT;
                        break;
                }
            }

            _saveLoadInstancesWatcher.RegisterProgress(weaponManagerInstance, lootManagerInstance, playerInstance);
            
            Context.StateMachine.SwitchState<DungeonRoomSetUpNavMeshState, MapDungeon>(mapDungeon);
        }
    }
}