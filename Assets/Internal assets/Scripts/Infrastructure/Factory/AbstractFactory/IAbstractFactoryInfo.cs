using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Factory.AbstractFactory
{
    public interface IAbstractFactoryInfo
    {
        public List<GameObject> Instances { get; }
    }
}