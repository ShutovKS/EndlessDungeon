#region

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class Attack : IState
    {
        public Attack(ref UnityAction<string> onAnimationTrigger, Animator animator, params EnemyDamage[] enemyDamages)
        {
            onAnimationTrigger += HandlerAnimationTrigger;
            _animator = animator;
            _enemyDamages = enemyDamages
                .Select(enemyDamage => enemyDamage.GetComponent<EnemyDamage>())
                .ToList();
        }

        private readonly static int Attack1 = Animator.StringToHash("Attack1");

        private readonly Animator _animator;

        private readonly List<EnemyDamage> _enemyDamages;

        public bool EndAttack;

        public void OnEnter()
        {
            _animator.SetBool(Attack1, true);
            EndAttack = false;
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
            _animator.SetBool(Attack1, false);
            foreach (var enemyDamage in _enemyDamages)
                enemyDamage.SetIsDamage(false);
        }

        private void HandlerAnimationTrigger(string animationTriggerName)
        {
            switch (animationTriggerName)
            {
                case "IsAttackStart":
                    foreach (var enemyDamage in _enemyDamages)
                        enemyDamage.SetIsDamage(true);

                    break;
                case "IsAttackEnd":
                    foreach (var enemyDamage in _enemyDamages)
                        enemyDamage.SetIsDamage(false);

                    break;
                case "AttackEnd":
                    EndAttack = true;
                    break;
            }
        }
    }
}