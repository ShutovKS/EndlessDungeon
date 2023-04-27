using Item;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        private int _health = 100;
    
        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ItemDamage>(out var damageComponent))
            {
                _health = damageComponent.Damage;
                if (_health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
