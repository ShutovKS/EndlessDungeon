using System;
using Infrastructure.Factory.EnemyFactory;
using UnityEngine;

namespace Units.Enemy.State_Machines.State
{
    public class Dead : IState
    {
        #region Variables

        private readonly Enemy _enemy;
        private readonly Animator _animator;
        private readonly IEnemyFactory _enemyFactory;
        private readonly static int DEAD = Animator.StringToHash("Dead");

        #endregion

        #region Constructors

        public Dead(Enemy enemy, Animator animator, IEnemyFactory enemyFactory)
        {
            _enemy = enemy;
            _animator = animator;
            _enemyFactory = enemyFactory;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            _animator.SetBool(DEAD, true);
            _enemyFactory.DeadEnemy(_enemy);
            Debug.Log("Dead");
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