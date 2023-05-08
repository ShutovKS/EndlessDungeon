using System.Collections.Generic;
using System.Linq;
using Data.Addressable;
using Data.Static;
using Services.AssetsAddressableService;
using Units.Enemy;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public StaticDataService(IAssetsAddressableService assetsAddressableService)
        {
            _assetsAddressableService = assetsAddressableService;
            LoadStaticData();
        }

        private readonly IAssetsAddressableService _assetsAddressableService;

        private readonly Dictionary<EnemyType, EnemyStaticData> _enemies = new()
        {
            { EnemyType.Golem, null }
        };

        public async void LoadStaticData()
        {
            _enemies[EnemyType.Golem] = await _assetsAddressableService
                .GetAsset<EnemyStaticData>(AssetsAddressablesConstants.GOLEM_DATA);
        }

        public EnemyStaticData GetEnemyData(EnemyType enemyType)
        {
            return _enemies.TryGetValue(enemyType, out var staticData) ? staticData : null;
        }
    }
}