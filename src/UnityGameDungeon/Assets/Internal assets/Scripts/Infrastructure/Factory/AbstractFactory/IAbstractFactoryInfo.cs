#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Infrastructure.Factory.AbstractFactory
{
    public interface IAbstractFactoryInfo
    {
        List<GameObject> Instances { get; }
    }
}
