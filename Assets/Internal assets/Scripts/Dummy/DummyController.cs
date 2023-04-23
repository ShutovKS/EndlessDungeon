using System;
using System.Collections;
using Item;
using UnityEngine;

namespace Dummy
{
    public class DummyController : MonoBehaviour
    {
        private static Animator _animator;
        private static BoxCollider _boxCollider;
        private static MeshCollider _meshCollider;
        private static readonly int Damage = Animator.StringToHash("Damage");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Reset = Animator.StringToHash("Reset");
        private static int _health = 100;
        private static bool _isDead = false;

        private int Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health > 0) return;
                _isDead = true;
                _animator.SetTrigger(Dead);
                StartCoroutine(Restart());
            }
        }

        private void Start()
        {
            if (_animator == null && TryGetComponent<Animator>(out var animator))
                _animator = animator;
            if (_boxCollider == null && TryGetComponent<BoxCollider>(out var boxCollider))
                _boxCollider = boxCollider;
            if (_meshCollider == null && TryGetComponent<MeshCollider>(out var meshCollider))
                _meshCollider = meshCollider;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_isDead && other.gameObject.TryGetComponent<ItemDamage>(out var itemDamage))
            {
                _animator.SetTrigger(Damage);
                Health -= itemDamage.Damage;
            }
        }

        private static IEnumerator Restart()
        {
            yield return new WaitForSeconds(5);
            _animator.SetTrigger(Reset);
        }

        public void Died()
        {
            _meshCollider.enabled = false;
            _boxCollider.enabled = true;
        }

        public void Revived()
        {
            _health = 100;
            _isDead = false;
            _meshCollider.enabled = true;
            _boxCollider.enabled = false;
        }
    }
}