using Units.Enemy;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data.Static
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }

        [field: SerializeField] public string Name { get; private set; }

        [field: Range(1, 100), SerializeField] public float MaxHealthPoints { get; private set; }

        [field: Range(1, 30), SerializeField] public float Damage { get; private set; }

        [field: Range(0.1f, 2), SerializeField]
        public float EffectiveDistance { get; private set; }

        [field: Range(0.1f, 2), SerializeField]
        public float Cleavage { get; private set; }

        [field: Range(0.1f, 10), SerializeField]
        public float AttackCooldown { get; private set; }

        [field: Range(0.1f, 10), SerializeField]
        public float MovementSpeed { get; private set; }

        [field: Range(10f, 100), SerializeField]
        public float RotationSpeed { get; private set; }

        [field: Space, SerializeField] public AssetReferenceGameObject PrefabReference { get; private set; }
    }
}