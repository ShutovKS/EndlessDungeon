#region

using UnityEngine;

#endregion

namespace Infrastructure.Factory.AbstractFactory
{
    public interface IAbstractFactory : IAbstractFactoryInfo
    {
        GameObject CreateInstance(GameObject prefab, Vector3 spawnPoint);
        void DestroyInstance(GameObject instance);
        void DestroyAllInstances();
    }
}
