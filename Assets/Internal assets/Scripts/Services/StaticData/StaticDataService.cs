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

        private readonly Dictionary<EnemyTypeId, EnemyStaticData> _enemies = new()
        {
            { EnemyTypeId.Golem, null }
        };

        public async void LoadStaticData()
        {
            _enemies[EnemyTypeId.Golem] = await _assetsAddressableService
                .GetAsset<EnemyStaticData>(AssetsAddressablesConstants.GOLEM_DATA);
        }

        public EnemyStaticData GetEnemyData(EnemyTypeId enemyTypeId)
        {
            return _enemies.TryGetValue(enemyTypeId, out var staticData) ? staticData : null;
        }
    }
}