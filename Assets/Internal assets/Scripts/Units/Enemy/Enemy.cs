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
        [NonSerialized] public bool PlayerInRange;
        public Action<string> AnimationTriggerName;
        private StateMachine _stateMachine;
        private float _healthPoints;

        public void SetUp(float healthPoints, float effectiveDistance, float cleavage, float attackCooldown,
            float damage, float movementSpeed, float rotationSpeed, Transform playerTransform, IEnemyFactory enemyFactory)
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
            foreach (var enemyOnTriggerEnter in GetComponents<EnemyGetHit>())
                enemyOnTriggerEnter.RegisterOnGetHit(getHitDamage => _healthPoints -= getHitDamage);

            _stateMachine = new StateMachine();

            var searchPositionForPatrol = new SearchPositionForPatrol(thisTransform, out var targetTransform);
            var patrol = new Patrol(animator, navMeshAgent, thisTransform, targetTransform, movementSpeed / 2);
            var combatReadiness = new CombatReadiness(animator, thisTransform, playerTransform, rotationSpeed);
            var attack = new Attack(this, animator, damage, enemyDamages);
            var getHit = new GetHit(this, animator);
            var dead = new Dead(this, animator, enemyFactory);
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
            At(dead, combatReadiness, IsDead());
            At(attack, combatReadiness, CanAttack());
            At(moveToPlayer, combatReadiness, PlayerNonInReachOfAttack());
            At(searchPositionForPatrol, moveToPlayer, PlayerNonInRangeVisible());
            At(combatReadiness, moveToPlayer, PlayerInReachOfAttack());
            At(moveToPlayer, patrol, PlayerInRangeVisible());
            At(searchPositionForPatrol, patrol, StuckForOverATwoSecondInPatrol());
            At(searchPositionForPatrol, patrol, ReachedPatrolPoint());
            AtAny(getHit, IsGetHit());
            // AtAny(dead, Failed());

            _stateMachine.SetState(searchPositionForPatrol);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            void AtAny(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

            Func<bool> StuckForOverATwoSecondInPatrol() => () => patrol.TimeStuck >= 2f;
            Func<bool> PlayerInRangeVisible() => () => playerTransform != null && PlayerInRange;
            Func<bool> PlayerNonInRangeVisible() => () => !PlayerInRange;
            Func<bool> CanAttack() => () => combatReadiness.TimePassed >= attackCooldown;
            Func<bool> AttackOver() => () => attack.EndAttack;
            Func<bool> GetHitOver() => () => getHit.EndGetHit;
            Func<bool> IsDead() => () => _healthPoints <= 0;
            // Func<bool> Failed() => () => thisTransform.position.y < -50;

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