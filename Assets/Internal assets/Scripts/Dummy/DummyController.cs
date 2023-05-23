#region

using System.Collections;
using UnityEngine;

#endregion

namespace Dummy
{
    public class DummyController : MonoBehaviour
    {
        private readonly static int Damage = Animator.StringToHash("Damage");
        private readonly static int Dead = Animator.StringToHash("Dead");
        private readonly static int Reset = Animator.StringToHash("Reset");

        [SerializeField] private float healthMax;
        [SerializeField] private float timeRestart;

        private Animator _animator;
        private float _health;
        private bool _isAction;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _health = healthMax;
            foreach (var dummyGetHit in GetComponentsInChildren<DummyGetHit>())
            {
                dummyGetHit.RegisterOnGetHit(TakeDamage);
            }
        }

        public void AnimationTriggerAction()
        {
            _isAction = true;
        }

        public void AnimationTriggerIdle()
        {
            _isAction = false;
        }

        public void AnimationTriggerRevived()
        {
            _health = healthMax;
        }

        private void TakeDamage(float damage)
        {
            if (_isAction) return;
            _health -= damage;

            if (_health > 0)
            {
                _animator.SetTrigger(Damage);
            }
            else
            {
                _animator.SetTrigger(Dead);
                StartCoroutine(Restart());
            }
        }

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(timeRestart);
            _animator.SetTrigger(Reset);
        }
    }
}