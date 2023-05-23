#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace TransitionToTheDungeon
{
    public class TransitionToTheDungeonTrigger : MonoBehaviour
    {
        private bool _isTrigger;
        private UnityAction _onTransitionToTheDungeon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isTrigger)
            {
                _onTransitionToTheDungeon?.Invoke();
                _isTrigger = true;
            }
        }

        public void RegisterOnTransitionToTheDungeon(UnityAction onTransitionToTheDungeon)
        {
            _onTransitionToTheDungeon = onTransitionToTheDungeon;
        }
        
        public void UnregisterOnTransitionToTheDungeon(UnityAction onTransitionToTheDungeon)
        {
            _onTransitionToTheDungeon -= onTransitionToTheDungeon;
        }
    }
}
