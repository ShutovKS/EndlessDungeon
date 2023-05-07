using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Factory.EnemyFactory
{
    public interface IEnemyFactoryInfo
    {
        public List<GameObject> Instances { get; }
    }
}