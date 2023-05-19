using Data.Dynamic;
using Data.Static;
using Services.PersistentProgress;
using Skill;
using UnityEngine;
using UnityEngine.Events;

namespace Units.Player
{
    public class Player : MonoBehaviour, IProgressLoadable
    {
        private PlayerStaticDefaultData _defaultData;
        private UnityAction _playerDead;
        private float _healthPoint;
        private float _protection;
        private bool _isDead;

        public void SetUp(UnityAction playerDead, PlayerStaticDefaultData playerStaticDefaultData)
        {
            _playerDead = playerDead;
            _defaultData = playerStaticDefaultData;

            var triggerGetHit = new GameObject
            {
                name = "TriggerGetHit",
                transform =
                {
                    parent = transform,
                    localPosition = Vector3.zero
                },
            };

            triggerGetHit.AddComponent<PlayerGetHit>().SetUp(TakeDamage);
        }

        private void TakeDamage(float healthLoss)
        {
            if (healthLoss - _protection < 0) return;

            _healthPoint -= (healthLoss - _protection);
            if (_healthPoint <= 0 && !_isDead)
            {
                _playerDead?.Invoke();
                _isDead = true;
            }
        }

        public void LoadProgress(Progress progress)
        {
            _healthPoint =
                (_defaultData.MaxHealthPoints + progress.skillsLevel.Skills[SkillType.HEALTH_Count]) *
                (1 + progress.skillsLevel.Skills[SkillType.HEALTH_Percent] / 100f);

            _protection =
                (_defaultData.ProtectionPoints + progress.skillsLevel.Skills[SkillType.PROTECTION_Count]) *
                (1 + progress.skillsLevel.Skills[SkillType.PROTECTION_Percent] / 100f);
        }
    }
}