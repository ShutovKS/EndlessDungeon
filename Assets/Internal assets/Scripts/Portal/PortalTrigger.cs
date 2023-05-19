using UnityEngine;
using UnityEngine.Events;

namespace Abstract
{
    public class PortalTrigger : MonoBehaviour
    {
        private UnityAction _action;
        private bool _isTrigger;

        public void SetUp(UnityAction action) => _action = action;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || _isTrigger) return;

            _action?.Invoke();
            _isTrigger = true;
        }
    }
}