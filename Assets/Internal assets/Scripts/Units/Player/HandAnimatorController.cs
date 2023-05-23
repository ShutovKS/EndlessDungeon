#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

namespace Units.Player
{
    public class HandAnimatorController : MonoBehaviour
    {
        private readonly static int Grip = Animator.StringToHash("Grip");
        private readonly static int Trigger = Animator.StringToHash("Trigger");
        [SerializeField] private InputActionProperty grabAction;
        [SerializeField] private InputActionProperty activateAction;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(Grip, grabAction.action.ReadValue<float>());
            _animator.SetFloat(Trigger, activateAction.action.ReadValue<float>());
        }
    }
}
