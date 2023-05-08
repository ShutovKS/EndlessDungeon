using System;
using System.Collections.Generic;
using Units.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Factory.EnemyFactory
{
    public interface IEnemyFactoryInfo
    {
        event UnityAction AllDeadEnemies;
        List<GameObject> Instances { get; }
        List<Enemy> Enemies { get; }
    }
}