using System;
using UnityEngine;

namespace Units.Enemy.State_Machines.State
{
    public class Dead : IState
    {
        #region Variables

        private readonly Animator _animator;
        private readonly static int DEAD = Animator.StringToHash("Dead");

        #endregion

        #region Constructors

        public Dead(Animator animator)
        {
            _animator = animator;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            _animator.SetBool(DEAD, true);
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
        }

        #endregion

        #region Methods Other

        #endregion
    }
}