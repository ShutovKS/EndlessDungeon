using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.State_Machines.State
{
    public class Attack : IState
    {
        #region Variables

        public bool EndAttack;

        private readonly Animator _animator;
        private readonly Enemy _enemy;
        private readonly static int ATTACK1 = Animator.StringToHash("Attack1");

        private readonly Dictionary<EnemyDamage.Side, EnemyDamage> _enemyDamages = new()
        {
            { EnemyDamage.Side.Left, null },
            { EnemyDamage.Side.Right, null },
        };

        private Action<string> _animationTriggerName;

        #endregion

        #region Constructors

        public Attack(Enemy enemy, Animator animator, float damage, params EnemyDamage[] enemyDamages)
        {
            _enemy = enemy;
            _animator = animator;

            foreach (var enemyDamage in enemyDamages)
            {
                enemyDamage.Damage = damage;
                _enemyDamages[enemyDamage.side] = enemyDamage;
            }
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            _animator.SetBool(ATTACK1, true);
            _enemy.AnimationTriggerName = HandlerAnimationTrigger;
            EndAttack = false;
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
            _animator.SetBool(ATTACK1, false);
            _enemy.AnimationTriggerName -= HandlerAnimationTrigger;
        }

        #endregion

        #region Methods Other

        private void HandlerAnimationTrigger(string animationTriggerName)
        {
            switch (animationTriggerName)
            {
                case "attackLeft":
                    _enemyDamages[EnemyDamage.Side.Left].SwitchCollider(true);
                    break;
                case "attackEndLeft":
                    _enemyDamages[EnemyDamage.Side.Left].SwitchCollider(false);
                    break;
                case "attackRight":
                    _enemyDamages[EnemyDamage.Side.Right].SwitchCollider(true);
                    break;
                case "attackEndRight":
                    _enemyDamages[EnemyDamage.Side.Right].SwitchCollider(false);
                    break;
                case "animationEnd":
                    EndAttack = true;
                    break;
            }
        }

        #endregion
    }
}