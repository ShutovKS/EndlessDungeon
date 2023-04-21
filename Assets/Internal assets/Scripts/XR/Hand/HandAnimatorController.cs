using UnityEngine;
using UnityEngine.InputSystem;

namespace XR.Hand
{
    public class HandAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private InputActionProperty _grabAction;
        [SerializeField] private InputActionProperty _activateAction;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat("Grip", _grabAction.action.ReadValue<float>());
            _animator.SetFloat("Trigger", _activateAction.action.ReadValue<float>());
        }
    }
}