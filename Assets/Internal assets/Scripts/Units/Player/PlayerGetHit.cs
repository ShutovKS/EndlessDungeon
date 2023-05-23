#region

using Units.Enemy;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Player
{
    public class PlayerGetHit : MonoBehaviour
    {
        private UnityAction<float> _onGetHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnemyDamage>(out var enemyDamage) && enemyDamage.IsDamage)
                _onGetHit?.Invoke(enemyDamage.Damage);
        }

        public void SetUp(UnityAction<float> onGetHit)
        {
            _onGetHit = onGetHit;

            var targetCollider = gameObject.AddComponent<CapsuleCollider>();
            targetCollider.isTrigger = true;
            targetCollider.center = new Vector3(0, 0.9f, 0);
            targetCollider.radius = 0.4f;
            targetCollider.height = 1.8f;
        }
    }
}
