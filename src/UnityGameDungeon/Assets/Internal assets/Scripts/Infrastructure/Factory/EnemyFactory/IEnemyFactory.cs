#region

using System.Threading.Tasks;
using Units.Enemy;
using UnityEngine;

#endregion

namespace Infrastructure.Factory.EnemyFactory
{
    public interface IEnemyFactory : IEnemyFactoryInfo
    {
        Task<GameObject> CreateInstance(EnemyType enemyType, Vector3 position);
        void DestroyInstance(GameObject instance);
        void DestroyAllInstances();
        void DeadEnemy(Enemy enemy);
    }
}
