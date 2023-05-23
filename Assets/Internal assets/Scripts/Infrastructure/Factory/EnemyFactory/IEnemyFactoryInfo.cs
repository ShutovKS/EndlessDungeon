#region

using System.Collections.Generic;
using Units.Enemy;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Infrastructure.Factory.EnemyFactory
{
    public interface IEnemyFactoryInfo
    {
        List<GameObject> Instances { get; }
        List<Enemy> Enemies { get; }
        event UnityAction AllDeadEnemies;
    }
}
