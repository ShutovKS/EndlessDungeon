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

            var dictionary = new Dictionary<Side, List<EnemyDamage>>();
            foreach (var damage in enemyDamages)
            {
                if (dictionary.TryGetValue(damage.Side, out var damages))
                {
                    damages.Add(damage);
                }
                else
                {
                    dictionary.Add(damage.Side, new List<EnemyDamage>());
                    dictionary[damage.Side].Add(damage);
                }
            }

            _enemyDamages = dictionary;
        }
        private readonly static int Attack1 = Animator.StringToHash("Attack1");

        private readonly Animator _animator;

        private readonly Dictionary<Side, List<EnemyDamage>> _enemyDamages;

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
        }

        private void HandlerAnimationTrigger(string animationTriggerName)
        {
            List<EnemyDamage> enemyDamages;
            switch (animationTriggerName)
            {
                case "attackLeft":
                    if (_enemyDamages.TryGetValue(Side.Left, out enemyDamages))
                        foreach (var enemyDamage in enemyDamages)
                            enemyDamage.SetIsDamage(true);

                    break;
                case "attackEndLeft":
                    if (_enemyDamages.TryGetValue(Side.Left, out enemyDamages))
                        foreach (var enemyDamage in enemyDamages)
                            enemyDamage.SetIsDamage(false);

                    break;
                case "attackRight":
                    if (_enemyDamages.TryGetValue(Side.Right, out enemyDamages))
                        foreach (var enemyDamage in enemyDamages)
                            enemyDamage.SetIsDamage(true);

                    break;
                case "attackEndRight":
                    if (_enemyDamages.TryGetValue(Side.Right, out enemyDamages))
                        foreach (var enemyDamage in enemyDamages)
                            enemyDamage.SetIsDamage(false);

                    break;
                case "animationEnd":
                    EndAttack = true;
                    break;
            }
        }
    }
}
