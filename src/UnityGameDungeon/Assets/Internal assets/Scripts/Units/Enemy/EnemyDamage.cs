#region

using UnityEngine;

#endregion

namespace Units.Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public bool IsDamage { get; private set; }

        public void SetIsDamage(bool value)
        {
            IsDamage = value;
        }
    }
}
