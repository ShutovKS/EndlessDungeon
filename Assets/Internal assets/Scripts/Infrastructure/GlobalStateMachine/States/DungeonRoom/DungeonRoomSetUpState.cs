using System;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Settings;
using GeneratorDungeons;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using Units.Enemy;
using Units.Player;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpState : StateOneParam<GameInstance, TileDungeon>
    {
        public DungeonRoomSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher, IEnemyFactory enemyFactory)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _enemyFactory = enemyFactory;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IEnemyFactory _enemyFactory;

        const float UNIT = 4.85f / 2;

        public override async void Enter(TileDungeon tileDungeon)
        {
            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var floor = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.FLOOR);
            var wall = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WALL);
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);

            var playerInstance = _abstractFactory.CreateInstance(player, Vector3.zero);
            var enemyDetectorInstance = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero);
            enemyDetectorInstance.transform.parent = playerInstance.transform;
            enemyDetectorInstance.AddComponent<EnemyDetector>();
            enemyDetectorInstance.AddComponent<SphereCollider>().radius = 7.5f;
            enemyDetectorInstance.GetComponent<SphereCollider>().isTrigger = true;
            var socketInstance = _abstractFactory.CreateInstance(
                socket,
                _mainLocationSettings.SocketForWeaponSpawnPosition);

            socketInstance.transform.parent = playerInstance.transform.GetChild(0).GetChild(0);

            for (var y = 0; y < tileDungeon.TilesMap.GetLength(0); y++)
            for (var x = 0; x < tileDungeon.TilesMap.GetLength(1); x++)
            {
                switch (tileDungeon.TilesMap[y, x])
                {
                    case MapTile.FLOOR:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        break;
                    case MapTile.WALL:
                        _abstractFactory.CreateInstance(wall, new Vector3(x, 0, y) * UNIT);
                        break;
                    case MapTile.ENTRY:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * UNIT);
                        playerInstance.transform.position = new Vector3(x, 0, y) * UNIT;
                        break;
                }
            }

            Context.StateMachine.SwitchState<DungeonRoomSetUpNavMeshState, TileDungeon>(tileDungeon);
        }
    }
}