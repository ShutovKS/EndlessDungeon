#region

using Item;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Dummy
{
    public class DummyGetHit : MonoBehaviour
    {
        private UnityAction<float> _onGetHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IItemDamage>(out var itemDamage) && itemDamage.IsDamage)
            {
                _onGetHit?.Invoke(itemDamage.Damage);
            }
        }

        public void RegisterOnGetHit(UnityAction<float> onGetHit)
        {
            _onGetHit = onGetHit;
        }
        
        public void UnregisterOnGetHit(UnityAction<float> onGetHit)
        {
            _onGetHit -= onGetHit;
        }
    }
}
