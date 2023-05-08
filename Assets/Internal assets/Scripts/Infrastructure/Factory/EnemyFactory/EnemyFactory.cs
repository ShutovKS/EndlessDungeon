using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Static;
using Services.AssetsAddressableService;
using Services.StaticData;
using Units.Enemy;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory.EnemyFactory
{
    public class EnemyFactory : IEnemyFactory
    {
        public EnemyFactory(DiContainer container, IAssetsAddressableService assetsAddressableService,
            IStaticDataService staticDataService)
        {
            _container = container;
            _assetsAddressableService = assetsAddressableService;
            _staticDataService = staticDataService;
        }

        private DiContainer _container;
        private IAssetsAddressableService _assetsAddressableService;
        private IStaticDataService _staticDataService;

        public List<GameObject> Instances => _instances;

        private List<GameObject> _instances = new();

        public async Task<GameObject> CreateInstance(EnemyType enemyType, Vector3 position)
        {
            var enemyStaticData = _staticDataService.GetEnemyData(enemyType);

            var prefab = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.GOLEM);

            var instance = _container.InstantiatePrefab(prefab, position, Quaternion.identity, null);

            SetUp(instance, enemyStaticData);

            _instances.Add(instance);
            
            return instance;
        }

        public void DestroyInstance(GameObject instance)
        {
            if (instance == null)
            {
                throw new NullReferenceException("There is no instance to destroy");
            }
            
            if (!_instances.Contains(instance))
            {
                throw new NullReferenceException($"Instance {instance} can't be destroyed, cause there is no {instance} on Abstract Factory Instances");
            }
            
            Object.Destroy(instance);
            _instances.Remove(instance);
        }

        public void DestroyAllInstances()
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                Object.Destroy(_instances[i]);
            }
        
            _instances.Clear();
        }

        public void DestroyAllInstances<T>(List<T> list) where T : Object
        {
            for (int i = 0; i < list.Count; i++)
            {
                Object.Destroy(list[i]);
            }
        
            list.Clear();
        }

        private void SetUp(GameObject instance, EnemyStaticData enemyStaticData)
        {
            if (instance.TryGetComponent(out Enemy enemy))
            {
                enemy.SetUp(
                    enemyStaticData.MaxHealthPoints,
                    enemyStaticData.EffectiveDistance,
                    enemyStaticData.Cleavage,
                    enemyStaticData.AttackCooldown,
                    enemyStaticData.MovementSpeed,
                    enemyStaticData.RotationSpeed);
            }
            else
            {
                instance.AddComponent<Enemy>().SetUp(
                    enemyStaticData.MaxHealthPoints,
                    enemyStaticData.EffectiveDistance,
                    enemyStaticData.Cleavage,
                    enemyStaticData.AttackCooldown,
                    enemyStaticData.MovementSpeed,
                    enemyStaticData.RotationSpeed);
            }
        }
    }
}