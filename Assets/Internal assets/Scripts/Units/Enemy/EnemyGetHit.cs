#region

using Item;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Enemy
{
    public class EnemyGetHit : MonoBehaviour
    {
        private UnityAction<float> _onGetHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IItemDamage>(out var itemDamage) && itemDamage.IsDamage)
                _onGetHit?.Invoke(itemDamage.Damage);
        }

        public void RegisterOnGetHitWatcher(UnityAction<float> action)
        {
            _onGetHit += action;
        }

        public void RemoveOnGetHitWatcher(UnityAction<float> action)
        {
            _onGetHit -= action;
        }
    }
}
