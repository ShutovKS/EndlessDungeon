using System;
using Infrastructure.Factory.EnemyFactory;
using Item;
using Units.Enemy.State_Machines;
using Units.Enemy.State_Machines.State;
using UnityEngine;
using UnityEngine.AI;

namespace Units.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private StateMachine _stateMachine;
        public Action<string> AnimationTriggerName;
        public bool playerInRange;
        private float _healthPoints;

        public void SetUp(float healthPoints, float effectiveDistance, float cleavage, float attackCooldown,
            float speedMove, float speedRotate, IEnemyFactory enemyFactory)
        {
            _healthPoints = healthPoints;
            var healthPointsLateUpdate = _healthPoints;
            var thisTransform = transform;
            var playerTransform = GameObject.FindWithTag("Player").transform;

            var animator = TryGetComponent<Animator>(out var animatorComponent)
                ? animatorComponent
                : gameObject.AddComponent<Animator>();

            var navMeshAgent = TryGetComponent<NavMeshAgent>(out var agent)
                ? agent
                : gameObject.AddComponent<NavMeshAgent>();

            _stateMachine = new StateMachine();

            var combatReadiness = new CombatReadiness(animator, thisTransform, playerTransform, speedRotate);
            var searchPositionForPatrol = new SearchPositionForPatrol(thisTransform, out var targetTransform);
            var patrol = new Patrol(animator, navMeshAgent, thisTransform, targetTransform, speedMove / 2);
            var attack = new Attack(this, animator);
            var getHit = new GetHit(this, animator);
            var dead = new Dead(this, animator, enemyFactory);

            var moveToPlayer = new MoveToPlayer(
                animator,
                navMeshAgent,
                thisTransform,
                playerTransform,
                speedMove,
                effectiveDistance);

            At(moveToPlayer, patrol, PlayerInRangeVisible());
            At(moveToPlayer, combatReadiness, PlayerNonInReachOfAttack());
            At(attack, combatReadiness, CanAttack());
            At(combatReadiness, moveToPlayer, PlayerInReachOfAttack());
            At(combatReadiness, attack, AttackOver());
            At(combatReadiness, getHit, GetHitOver());
            At(patrol, searchPositionForPatrol, HasTargetForPatrol());
            At(searchPositionForPatrol, moveToPlayer, PlayerNonInRangeVisible());
            At(searchPositionForPatrol, patrol, StuckForOverATwoSecondInPatrol());
            At(searchPositionForPatrol, patrol, ReachedPatrolPoint());
            AtAny(getHit, IsGetHit());
            AtAny(dead, IsDead());
            AtAny(dead, Failed());

            _stateMachine.SetState(searchPositionForPatrol);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            void AtAny(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

            Func<bool> StuckForOverATwoSecondInPatrol() => () => patrol.TimeStuck >= 2f;
            Func<bool> PlayerInRangeVisible() => () => playerTransform != null && playerInRange;
            Func<bool> PlayerNonInRangeVisible() => () => !playerInRange;
            Func<bool> CanAttack() => () => combatReadiness.TimePassed >= attackCooldown;
            Func<bool> AttackOver() => () => attack.EndAttack;
            Func<bool> GetHitOver() => () => getHit.EndGetHit;
            Func<bool> IsDead() => () => _healthPoints <= 0;
            Func<bool> Failed() => () => thisTransform.position.y < -50;

            Func<bool> PlayerInReachOfAttack() => () =>
                Vector3.Distance(thisTransform.position, playerTransform.position) <= effectiveDistance * 0.9;

            Func<bool> PlayerNonInReachOfAttack() => () =>
                Vector3.Distance(thisTransform.position, playerTransform.position) > effectiveDistance * 1.1;

            Func<bool> HasTargetForPatrol() => () =>
                Vector3.Distance(thisTransform.position, targetTransform.position) > 5f;

            Func<bool> ReachedPatrolPoint() => () =>
                Vector3.Distance(thisTransform.position, targetTransform.position) < 1f;

            Func<bool> IsGetHit() => () =>
            {
                if (_healthPoints == healthPointsLateUpdate || _healthPoints <= 0)
                    return false;

                healthPointsLateUpdate = _healthPoints;
                return true;
            };
        }

        private void Update() => _stateMachine?.Tick();
        public void AnimationTrigger(string triggerName) => AnimationTriggerName?.Invoke(triggerName);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IItemDamage>(out var itemDamage) && itemDamage.IsDamage)
                _healthPoints -= itemDamage.Damage;
        }
    }
}