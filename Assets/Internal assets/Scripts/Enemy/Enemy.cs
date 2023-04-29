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

        private void Awake()
        {
            var thisTransform = transform;
            var targetTransform = new GameObject().transform;
            var animator = GetComponent<Animator>();
            var navMeshAgent = TryGetComponent<NavMeshAgent>(out var agent)
                ? agent
                : gameObject.AddComponent<NavMeshAgent>();

            _stateMachine = new StateMachine();

            var searchPositionForPatrol = new SearchPositionForPatrol(thisTransform, ref targetTransform);
            var patrol = new Patrol(animator, navMeshAgent, thisTransform, ref targetTransform);

            At(patrol, searchPositionForPatrol, HasTarget());
            At(searchPositionForPatrol, patrol, StuckForOverATwoSecond());
            At(searchPositionForPatrol, patrol, ReachedPatrolPoint());

            _stateMachine.SetState(searchPositionForPatrol);

            void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
            void AtAny(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

            Func<bool> StuckForOverATwoSecond() => () => patrol.TimeStuck > 2f;

            Func<bool> HasTarget() => () =>
                Vector3.Distance(transform.position, targetTransform.position) > 5f;

            Func<bool> ReachedPatrolPoint() => () =>
                Vector3.Distance(transform.position, targetTransform.position) < 1f;
        }

        private void Update() => _stateMachine.Tick();
    }
}