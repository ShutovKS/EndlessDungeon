using Units.Enemy;
using UnityEngine;

namespace Data.Static
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [field: Range(1, 100)]
        [field: SerializeField]
        public float MaxHealthPoints { get; private set; }

        [field: Range(1, 30)]
        [field: SerializeField]
        public float Damage { get; private set; }

        [field: Range(0.1f, 2)]
        [field: SerializeField]
        public float EffectiveDistance { get; private set; }

        [field: Range(0.1f, 2)]
        [field: SerializeField]
        public float Cleavage { get; private set; }

        [field: Range(0.1f, 10)]
        [field: SerializeField]
        public float AttackCooldown { get; private set; }

        [field: Range(0.1f, 10)]
        [field: SerializeField]
        public float MovementSpeed { get; private set; }
        
        [field: Range(10f, 100)]
        [field: SerializeField]
        public float RotationSpeed { get; private set; }
    }
}