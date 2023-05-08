using System.Threading.Tasks;
using Infrastructure.Factory.Model;
using Units.Enemy;
using UnityEngine;

namespace Infrastructure.Factory.EnemyFactory
{
    public interface IEnemyFactory: IEnemyFactoryInfo, IFactory
    {
        Task<GameObject> CreateInstance(EnemyType enemyType, Vector3 position);
    }
}