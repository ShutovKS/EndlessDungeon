using Data.Static;
using Units.Enemy;

namespace Services.StaticData
{
    public interface IStaticDataService
    {
        public EnemyStaticData GetEnemyData(EnemyType enemyType);
    }
}