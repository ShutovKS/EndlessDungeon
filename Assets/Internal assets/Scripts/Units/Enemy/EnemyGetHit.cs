using Item;
using UnityEngine;
using UnityEngine.Events;

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

        public void RegisterOnGetHit(UnityAction<float> action)
        {
            _onGetHit += action;
        }
        
        public void RemoveOnGetHit(UnityAction<float> action)
        {
            _onGetHit -= action;
        }
    }
}