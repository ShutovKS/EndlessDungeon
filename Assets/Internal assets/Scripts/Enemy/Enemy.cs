using System;
using Enemy.State_Machines;
using Enemy.State_Machines.State;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        private StateMachine _stateMachine;
        public Action<string> AnimationTriggerName;

        private void Awake()
        {
            var thisTransform = transform;
            var playerTransform = GameObject.FindWithTag("Player").transform;
            var animator = GetComponent<Animator>();
            var navMeshAgent = TryGetComponent<NavMeshAgent>(out var agent)
                ? agent
                : gameObject.AddComponent<NavMeshAgent>();
            var playerDetector = new GameObject { transform = { parent = transform, localPosition = Vector3.zero } }
                .AddComponent<PlayerDetector>();

            _stateMachine = new StateMachine();

            var searchPositionForPatrol = new SearchPositionForPatrol(thisTransform, out var targetTransform);
            var patrol = new Patrol(animator, navMeshAgent, thisTransform, targetTransform);
            var goToPlayer = new GoToPlayer(animator, navMeshAgent, thisTransform, playerTransform);
            var combatReadiness = new CombatReadiness(animator);
            var attack = new Attack(this, animator);

            At(goToPlayer, patrol, PlayerInRangeVisible());
            At(goToPlayer, combatReadiness, PlayerNonInReachOfAttack());
            At(attack, combatReadiness, CanAttack());
            At(combatReadiness, goToPlayer, PlayerInReachOfAttack());
            At(combatReadiness, attack, AttackOver());
            At(patrol, searchPositionForPatrol, HasTargetForPatrol());
            At(searchPositionForPatrol, goToPlayer, PlayerNonInRangeVisible());
            At(searchPositionForPatrol, patrol, StuckForOverATwoSecondInPatrol());
            At(searchPositionForPatrol, patrol, ReachedPatrolPoint());

            _stateMachine.SetState(searchPositionForPatrol);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            // void AtAny(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

            Func<bool> StuckForOverATwoSecondInPatrol() => () => patrol.TimeStuck >= 2f;
            Func<bool> PlayerInRangeVisible() => () => playerTransform != null && playerDetector.PlayerInRange;
            Func<bool> PlayerNonInRangeVisible() => () => !playerDetector.PlayerInRange;
            Func<bool> CanAttack() => () => combatReadiness.TimePassed >= 5;
            Func<bool> AttackOver() => () => attack.attackEnd;


            Func<bool> PlayerInReachOfAttack() =>
                () => Vector3.Distance(thisTransform.position, playerTransform.position) <= 1.75f;

            Func<bool> PlayerNonInReachOfAttack() =>
                () => Vector3.Distance(thisTransform.position, playerTransform.position) > 2.75f;

            Func<bool> HasTargetForPatrol() =>
                () => Vector3.Distance(thisTransform.position, targetTransform.position) > 5f;

            Func<bool> ReachedPatrolPoint() => () =>
                Vector3.Distance(thisTransform.position, targetTransform.position) < 1f;
        }

        private void Update() => _stateMachine.Tick();

        public void AnimationTrigger(string triggerName)
        {
            AnimationTriggerName?.Invoke(triggerName);
        }
    }
}