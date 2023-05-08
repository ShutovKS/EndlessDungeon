using System;
using UnityEngine;

namespace Units.Enemy.State_Machines.State
{
    public class GetHit : IState
    {
        #region Variables

        public bool EndGetHit;

        private readonly Animator _animator;
        private readonly Enemy _enemy;
        private readonly static int DAMAGE = Animator.StringToHash("Damage");

        private Action<string> _animationTriggerName;

        #endregion

        #region Constructors

        public GetHit(Enemy enemy, Animator animator)
        {
            _enemy = enemy;
            _animator = animator;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            EndGetHit = false;
            _animator.SetBool(DAMAGE, true);
            _enemy.AnimationTriggerName = HandlerAnimationTrigger;
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
            _animator.SetBool(DAMAGE, false);
            _enemy.AnimationTriggerName -= HandlerAnimationTrigger;
        }

        #endregion

        #region Methods Other

        private void HandlerAnimationTrigger(string animationTriggerName)
        {
            Debug.Log("animationEnd Damage");
            if (animationTriggerName == "animationEnd")
                EndGetHit = true;
        }

        #endregion
    }
}