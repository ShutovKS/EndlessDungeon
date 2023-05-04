using UnityEngine;
using UnityEngine.InputSystem;

namespace XR.Hand
{
    public class HandAnimatorController : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _grabAction;
        [SerializeField] private InputActionProperty _activateAction;
        private Animator _animator;

        private static readonly int GRIP = Animator.StringToHash("Grip");
        private static readonly int TRIGGER = Animator.StringToHash("Trigger");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(GRIP, _grabAction.action.ReadValue<float>());
            _animator.SetFloat(TRIGGER, _activateAction.action.ReadValue<float>());
        }
    }
}