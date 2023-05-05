using System;
using Data.Addressable;
using Data.Settings;
using GeneratorDungeons;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.AssetsAddressableService;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpState : StateOneParam<GameInstance, TileDungeon.Tile[,]>
    {
        public DungeonRoomSetUpState(GameInstance context, IAbstractFactory abstractFactory,
            IAssetsAddressableService assetsAddressableService, MainLocationSettings mainLocationSettings,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher)
            : base(context)
        {
            _abstractFactory = abstractFactory;
            _assetsAddressableService = assetsAddressableService;
            _mainLocationSettings = mainLocationSettings;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
        }

        private readonly IAbstractFactory _abstractFactory;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly MainLocationSettings _mainLocationSettings;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        public override async void Enter(TileDungeon.Tile[,] tileDungeon)
        {
            const float unit = 4.85f / 2;

            var player = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.PLAYER);
            var floor = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.FLOOR);
            var wall = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.WALL);
            var socket =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.SOCKET_FOR_SWORD);

            GameObject playerInstance;
            GameObject socketInstance;

            for (var y = 0; y < tileDungeon.GetLength(0); y++)
            for (var x = 0; x < tileDungeon.GetLength(1); x++)
            {
                switch (tileDungeon[y, x])
                {
                    case TileDungeon.Tile.FLOOR:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * unit);
                        break;
                    case TileDungeon.Tile.INTERIOR_WALL:
                        _abstractFactory.CreateInstance(wall, new Vector3(x, 0, y) * unit);
                        break;
                    case TileDungeon.Tile.EXTERIOR_WALL:
                        break;
                    case TileDungeon.Tile.ENTRY:
                        _abstractFactory.CreateInstance(floor, new Vector3(x, 0, y) * unit);
                        playerInstance = _abstractFactory.CreateInstance(player, new Vector3(x, 0, y) * unit);
                        socketInstance = _abstractFactory.CreateInstance(socket,
                            new Vector3(x, 0, y) * unit + _mainLocationSettings.SocketForWeaponSpawnPosition);
                        socketInstance.transform.parent = playerInstance.transform.GetChild(0).GetChild(0);
                        break;
                    case TileDungeon.Tile.EXIT:
                        break;
                }
            }
            
            Context.StateMachine.SwitchState<DungeonRoomState>();
        }
    }
}