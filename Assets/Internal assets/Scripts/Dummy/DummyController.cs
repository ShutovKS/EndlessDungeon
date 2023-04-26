using System.Collections;
using Item;
using UnityEngine;

namespace Dummy
{
    public class DummyController : MonoBehaviour
    {
        private static readonly int Damage = Animator.StringToHash("Damage");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Reset = Animator.StringToHash("Reset");

        private Animator _animator;

        private int _health = 1000;
        private bool _isAction = false;

        private int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health > 0) return;
                _animator.SetTrigger(Dead);
                StartCoroutine(Restart());
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_isAction && other.gameObject.TryGetComponent<ItemDamage>(out var itemDamage))
            {
                if(itemDamage.isDamage)
                { 
                        _animator.SetTrigger(Damage);
                        Health -= itemDamage.Damage;
                }
            }
        }

        //using animation
        public void Action() => _isAction = true;
        public void Idle() => _isAction = false;
        public void Revived() => _health = 1000;

        private IEnumerator Restart()
        {
            yield return new WaitForSeconds(3.5f);
            _animator.SetTrigger(Reset);
        }
    }
}