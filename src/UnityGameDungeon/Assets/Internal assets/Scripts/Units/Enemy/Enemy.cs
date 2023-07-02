#region

using System;
using Units.Enemy.State_Machines;
using Units.Enemy.State_Machines.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

#endregion

namespace Units.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _healthPoints;
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private UnityAction<string> _onAnimationTriggerName;
        [SerializeField] public bool PlayerInRange;

        private void Update()
        {
            _stateMachine?.Tick();
        }

        public void SetUp(float healthPoints, float effectiveDistance, float attackCooldown,
            float damage, float movementSpeed, float rotationSpeed, Transform playerTransform,
            UnityAction<Enemy> onEnemyDead)
        {
            _healthPoints = healthPoints;
            var healthPointsLateUpdate = _healthPoints;

            var thisTransform = transform;

            var animator = TryGetComponent<Animator>(out var animatorComponent)
                ? animatorComponent
                : gameObject.AddComponent<Animator>();

            var navMeshAgent = TryGetComponent<NavMeshAgent>(out var agent)
                ? agent
                : gameObject.AddComponent<NavMeshAgent>();

            var enemyDamages = GetComponentsInChildren<EnemyDamage>();
            foreach (var enemyDamage in enemyDamages)
            {
                enemyDamage.Damage = damage;
            }

            foreach (var enemyGetHit in GetComponentsInChildren<EnemyGetHit>())
            {
                enemyGetHit.RegisterOnGetHitWatcher(
                    getHitDamage =>
                    {
                        _healthPoints -= getHitDamage;
                        Debug.Log($"{_healthPoints}/{healthPointsLateUpdate}");
                    });
            }

            _stateMachine = new StateMachine();

            var searchPositionForPatrol = new SearchPositionForPatrol(thisTransform, out var targetTransform);
            var patrol = new Patrol(animator, navMeshAgent, thisTransform, targetTransform, movementSpeed / 2);
            var combatReadiness = new CombatReadiness(thisTransform, playerTransform, rotationSpeed);
            var attack = new Attack(ref _onAnimationTriggerName, animator, enemyDamages);
            var getHit = new GetHit(ref _onAnimationTriggerName, animator);
            var dead = new Dead(this, animator, onEnemyDead);
            var moveToPlayer = new MoveToPlayer(
                animator,
                navMeshAgent,
                thisTransform,
                playerTransform,
                movementSpeed,
                effectiveDistance);

            At(combatReadiness, attack, AttackOver());
            At(combatReadiness, getHit, GetHitOver());
            At(patrol, searchPositionForPatrol, HasTargetForPatrol());
            At(attack, combatReadiness, CanAttack());
            At(moveToPlayer, combatReadiness, PlayerNonInReachOfAttack());
            At(searchPositionForPatrol, moveToPlayer, StuckForOverATwoSecondInMoveToPlayer());
            At(searchPositionForPatrol, moveToPlayer, PlayerNonInRangeVisible());
            At(combatReadiness, moveToPlayer, PlayerInReachOfAttack());
            At(moveToPlayer, patrol, PlayerInRangeVisible());
            At(searchPositionForPatrol, patrol, StuckForOverATwoSecondInPatrol());
            At(searchPositionForPatrol, patrol, ReachedPatrolPoint());
            At(dead, getHit, IsDead());
            AtAny(dead, Failed());
            AtAny(getHit, IsGetHit());

            _stateMachine.SetState(searchPositionForPatrol);

            void At(IState to, IState from, Func<bool> condition)
            {
                _stateMachine.AddTransition(to, from, condition);
            }

            void AtAny(IState to, Func<bool> condition)
            {
                _stateMachine.AddAnyTransition(to, condition);
            }

            Func<bool> StuckForOverATwoSecondInPatrol()
            {
                return () => patrol.TimeStuck >= 2f;
            }

            Func<bool> StuckForOverATwoSecondInMoveToPlayer()
            {
                return () => moveToPlayer.TimeStuck >= 2f;
            }

            Func<bool> PlayerInRangeVisible()
            {
                return () => playerTransform != null && PlayerInRange;
            }

            Func<bool> PlayerNonInRangeVisible()
            {
                return () => !PlayerInRange;
            }

            Func<bool> CanAttack()
            {
                return () => combatReadiness.TimePassed >= attackCooldown;
            }

            Func<bool> AttackOver()
            {
                return () => attack.EndAttack;
            }

            Func<bool> GetHitOver()
            {
                return () => getHit.EndGetHit;
            }

            Func<bool> IsDead()
            {
                return () => _healthPoints <= 0;
            }

            Func<bool> Failed()
            {
                return () => thisTransform.position.y < -50;
            }

            Func<bool> PlayerInReachOfAttack()
            {
                return () =>
                    Vector3.Distance(thisTransform.position, playerTransform.position) <= effectiveDistance * 0.9;
            }

            Func<bool> PlayerNonInReachOfAttack()
            {
                return () =>
                    Vector3.Distance(thisTransform.position, playerTransform.position) > effectiveDistance * 1.1;
            }

            Func<bool> HasTargetForPatrol()
            {
                return () => Vector3.Distance(thisTransform.position, targetTransform.position) > 5f;
            }

            Func<bool> ReachedPatrolPoint()
            {
                return () => Vector3.Distance(thisTransform.position, targetTransform.position) < 1f;
            }

            Func<bool> IsGetHit()
            {
                return () =>
                {
                    if (_healthPoints == healthPointsLateUpdate || _healthPoints <= 0)
                        return false;

                    healthPointsLateUpdate = _healthPoints;
                    return true;
                };
            }
        }

        public void AnimationTrigger(string triggerName)
        {
            _onAnimationTriggerName?.Invoke(triggerName);
        }
    }
}