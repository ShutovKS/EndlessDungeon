#region

using UnityEngine;
using UnityEngine.Events;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class Dead : IState
    {
        public Dead(Enemy enemy, Animator animator, UnityAction<Enemy> onEnemyDead)
        {
            _enemy = enemy;
            _animator = animator;
            _onEnemyDead = onEnemyDead;
        }
        private readonly static int Death = Animator.StringToHash("Dead");
        private readonly Animator _animator;
        private readonly Enemy _enemy;
        private readonly UnityAction<Enemy> _onEnemyDead;

        public void OnEnter()
        {
            _animator.SetBool(Death, true);
            _onEnemyDead?.Invoke(_enemy);
        }

        public void Tick()
        {
        }

        public void OnExit()
        {
        }
    }
}
