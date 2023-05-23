#region

using System.Collections.Generic;
using System.Linq;
using Data.Static;
using Units.Enemy;
using UnityEngine;

#endregion

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public StaticDataService()
        {
            LoadStaticData();
        }

        private const string ENEMIES_STATIC_DATA_PATH = "Data/Static/Enemies";

        private Dictionary<EnemyType, EnemyStaticData> _enemies;

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
