using UnityEngine;

namespace Enemy.State_Machines.State
{
    public class CombatReadiness : IState
    {
        #region Variables   

        public float TimePassed;
        private readonly Animator _animator;

        #endregion

        #region Constructors

        public CombatReadiness(Animator animator)
        {
            _animator = animator;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            TimePassed = 0f;
        }

        public void Tick()
        {
            TimePassed += Time.deltaTime;
        }

        public void OnExit()
        {
        }

        #endregion

        #region Methods Other

        #endregion
    }
}