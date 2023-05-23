#region

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

#endregion

namespace Infrastructure.Factory.AbstractFactory
{
    public class AbstractFactory : IAbstractFactory
    {
        public AbstractFactory(DiContainer container)
        {
            _container = container;
        }

        private readonly DiContainer _container;

        public List<GameObject> Instances { get; } = new();

        public GameObject CreateInstance(GameObject prefab, Vector3 spawnPoint)
        {
            var instance = _container.InstantiatePrefab(prefab, spawnPoint, Quaternion.identity, null);

            Instances.Add(instance);

            return instance;
        }

        public void DestroyInstance(GameObject instance)
        {
            if (instance == null)
            {
                throw new NullReferenceException("There is no instance to destroy");
            }

            if (!Instances.Contains(instance))
            {
                throw new NullReferenceException(
                    $"Instance {instance} can't be destroyed, cause there is no {instance} on Abstract Factory Instances");
            }

            Object.Destroy(instance);
            Instances.Remove(instance);
        }

        public void DestroyAllInstances()
        {
            foreach (var gameObject in Instances)
            {
                Object.Destroy(gameObject);
            }

            Instances.Clear();
        }
    }
}
