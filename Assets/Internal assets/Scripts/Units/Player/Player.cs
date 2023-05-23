#region

using Data.Dynamic;
using Services.Watchers.PersistentProgressWatcher;
using Skill;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Player
{
    public class Player : MonoBehaviour, IProgressLoadableWatcher
    {
        private float _healthPoints;
        private bool _isDead;
        private UnityAction _playerDead;
        private float _protectionPoints;

        public void LoadProgress(Progress progress)
        {
            _healthPoints =
                (_healthPoints + progress.skillsLevel.skills[SkillType.HealthCount]) *
                (1 + progress.skillsLevel.skills[SkillType.HealthPercent] / 100f);

            _protectionPoints =
                (_protectionPoints + progress.skillsLevel.skills[SkillType.ProtectionCount]) *
                (1 + progress.skillsLevel.skills[SkillType.ProtectionPercent] / 100f);
        }

        public void SetUp(float health, float protection)
        {
            _healthPoints = health;
            _protectionPoints = protection;

            var triggerGetHit = new GameObject
            {
                name = "TriggerGetHit",
                transform =
                {
                    parent = transform,
                    localPosition = Vector3.zero
                }
            };

            triggerGetHit.AddComponent<PlayerGetHit>().SetUp(TakeDamage);
        }

        public void RegisterOnPlayerDead(UnityAction playerDead)
        {
            _playerDead += playerDead;
        }

        private void TakeDamage(float damage)
        {
            if (_protectionPoints > damage) return;

            _healthPoints -= damage - _protectionPoints;
            if (_healthPoints <= 0 && !_isDead)
            {
                _playerDead?.Invoke();
                _isDead = true;
            }
        }
    }
}
