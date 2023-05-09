using System;
using UnityEngine;

namespace Units.Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        public float Damage { get; set; }
        [field: SerializeField] public Side side { get; private set; }

        private Collider _damageCollider;

        private void Start()
        {
            if (TryGetComponent<Collider>(out var damageCollider))
            {
                _damageCollider = damageCollider;
                _damageCollider.isTrigger = enabled;
                SwitchCollider(false);
            }
        }

        public void SwitchCollider(bool value) => _damageCollider.enabled = value;

        public enum Side
        {
            Left,
            Right,
            Other
        }
    }
}