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
        private const string ENEMIES_STATIC_DATA_PATH = "Data/Static/Enemies";

        private Dictionary<EnemyType, EnemyStaticData> _enemies;

        public StaticDataService(IAssetsAddressableService assetsAddressableService)
        {
            _assetsAddressableService = assetsAddressableService;
            LoadStaticData();
        }

        private readonly IAssetsAddressableService _assetsAddressableService;

        public void LoadStaticData()
        {
            _enemies = Resources.LoadAll<EnemyStaticData>(ENEMIES_STATIC_DATA_PATH).ToDictionary(
                enemyStaticData => enemyStaticData.EnemyType,
                enemyStaticData => enemyStaticData);
        }

        public EnemyStaticData GetEnemyData(EnemyType enemyType)
        {
            return _enemies.TryGetValue(enemyType, out var staticData) ? staticData : null;
        }
    }
}