#region

using Data.Static;
using Units.Enemy;

#endregion

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        public void LoadStaticData();
        public EnemyStaticData GetEnemyData(EnemyType enemyType);
    }
}
