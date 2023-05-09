using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Addressable;
using Data.Static;
using Services.AssetsAddressableService;
using Services.StaticData;
using Units.Enemy;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Object = UnityEngine.Object;

namespace Infrastructure.Factory.EnemyFactory
{
    public class EnemyFactory : IEnemyFactory
    {
        public EnemyFactory(DiContainer container,
            IAssetsAddressableService assetsAddressableService,
            IStaticDataService staticDataService)
        {
            _container = container;
            _assetsAddressableService = assetsAddressableService;
            _staticDataService = staticDataService;
        }

        private readonly DiContainer _container;
        private readonly IAssetsAddressableService _assetsAddressableService;
        private readonly IStaticDataService _staticDataService;

        public event UnityAction AllDeadEnemies;
        public List<GameObject> Instances { get; } = new();
        public List<Enemy> Enemies { get; } = new();

        public async Task<GameObject> CreateInstance(EnemyType enemyType, Vector3 position)
        {
            var enemyStaticData = _staticDataService.GetEnemyData(enemyType);

            var prefab = await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.GOLEM);

            var instance = _container.InstantiatePrefab(prefab, position, Quaternion.identity, null);

            SetUp(instance, enemyStaticData);

            Enemies.Add(instance.GetComponent<Enemy>());
            Instances.Add(instance);

            return instance;
        }

        public void DeadEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
            if (Enemies.Count == 0)
                AllDeadEnemies?.Invoke();
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

            Enemies.Remove(instance.GetComponent<Enemy>());
            Object.Destroy(instance);
            Instances.Remove(instance);
        }

        public void DestroyAllInstances()
        {
            foreach (var value in Instances)
            {
                Object.Destroy(value);
            }

            Enemies.Clear();
            Instances.Clear();
        }

        public void DestroyAllInstances<T>(List<T> list) where T : Object
        {
            foreach (var value in list)
            {
                Object.Destroy(value);
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
                    enemyStaticData.Damage,
                    enemyStaticData.MovementSpeed,
                    enemyStaticData.RotationSpeed,
                    this);
            }
            else
            {
                instance.AddComponent<Enemy>().SetUp(
                    enemyStaticData.MaxHealthPoints,
                    enemyStaticData.EffectiveDistance,
                    enemyStaticData.Cleavage,
                    enemyStaticData.AttackCooldown,
                    enemyStaticData.Damage,
                    enemyStaticData.MovementSpeed,
                    enemyStaticData.RotationSpeed,
                    this);
            }
        }
    }
}