using Infrastructure.Factory.Model;
using UnityEngine;
using Zenject;
using IFactory = Infrastructure.Factory.Model.IFactory;

namespace Infrastructure.Factory.AbstractFactory
{
    public interface IAbstractFactory : IAbstractFactoryInfo, IFactory
    {
        public GameObject CreateInstance(GameObject prefab, Vector3 spawnPoint);
    }
}