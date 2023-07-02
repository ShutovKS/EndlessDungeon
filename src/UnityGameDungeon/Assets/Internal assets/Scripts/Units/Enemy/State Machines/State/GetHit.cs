#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class GetHit : IState
    {
        public GetHit(ref UnityAction<string> onAnimationTrigger, Animator animator)
        {
            onAnimationTrigger += HandlerAnimationTrigger;
            _animator = animator;
        }
        private readonly static int Damage = Animator.StringToHash("GetHit");
        private readonly Animator _animator;
        public bool EndGetHit;

        public void OnEnter()
        {
            EndGetHit = false;
            _animator.SetBool(Damage, true);
            Debug.Log("GetHit");
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
            _animator.SetBool(Damage, false);
        }

        private void HandlerAnimationTrigger(string animationTriggerName)
        {
            if (animationTriggerName == "GetHitEnd")
            {
                EndGetHit = true;
            }
        }
    }
}
