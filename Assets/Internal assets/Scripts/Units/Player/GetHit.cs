using Units.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Player
{
    public class GetHit : MonoBehaviour
    {
        private UnityAction<float> _onGetHit;

        public void SetUp(UnityAction<float> onGetHit)
        {
            _onGetHit = onGetHit;

            var targetCollider = gameObject.AddComponent<CapsuleCollider>();
            targetCollider.isTrigger = true;
            targetCollider.center = new Vector3(0, 0.9f, 0);
            targetCollider.radius = 0.4f;
            targetCollider.height = 1.8f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnemyDamage>(out var enemyDamage))
                _onGetHit?.Invoke(enemyDamage.Damage);
        }
    }
}