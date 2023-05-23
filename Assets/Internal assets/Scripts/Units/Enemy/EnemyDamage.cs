#region

using UnityEngine;

#endregion

namespace Units.Enemy
{
    public class EnemyDamage : MonoBehaviour
    {
        [field: SerializeField] public Side Side { get; private set; }
        public float Damage { get; set; }
        public bool IsDamage { get; private set; }

        public void SetIsDamage(bool value)
        {
            IsDamage = value;
        }
    }
}
