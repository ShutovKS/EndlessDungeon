using UnityEngine;

namespace Units.Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        public float Damage { get; set; }
        public bool IsDamage { get; private set; }
        [field: SerializeField] public Side side { get; private set; }
        
        public void SwitchCollider(bool value) => IsDamage = value;

        public enum Side
        {
            Left,
            Right,
        }
    }
}