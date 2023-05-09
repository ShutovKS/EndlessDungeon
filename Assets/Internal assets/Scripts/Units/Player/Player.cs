using Data.Dynamic.PlayerData;
using Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Player
{
    public class Player : MonoBehaviour, IProgressLoadable
    {
        private UnityAction _playerDead;
        private float _healthPoint;
        private bool _isDead;

        public void SetUp(UnityAction playerDead)
        {
            _playerDead = playerDead;

            var triggerGetHit = new GameObject("TriggerGetHit")
            {
                transform =
                {
                    parent = transform,
                    localPosition = Vector3.zero
                }
            };

            triggerGetHit.AddComponent<GetHit>().SetUp(TakeDamage);
        }

        private void TakeDamage(float healthLoss)
        {
            _healthPoint -= healthLoss;
            if (_healthPoint <= 0 && !_isDead)
            {
                _playerDead?.Invoke();
                _isDead = true;
            }

            Debug.Log(_healthPoint);
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            //TODO: load healthPoints
            // _healthPoint = playerProgress.
        }
    }
}