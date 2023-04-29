using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.State_Machines.State
{
    public class Patrol : IState
    {
        #region Variables

        public float TimeStuck;

        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _thisTransform;
        private readonly Transform _targetTransform;
        private Vector3 _lastPosition = Vector3.zero;
        
        private static readonly int Walk = Animator.StringToHash("Walk");

        #endregion

        #region Constructors

        public Patrol(Animator animator, NavMeshAgent navMeshAgent, Transform thisTransform, ref Transform targetTransform)
        {
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _thisTransform = thisTransform;
            _targetTransform = targetTransform;
        }

        #endregion

        #region Methods Unity

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = 4;
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.SetDestination(_targetTransform.position);

            _animator.SetBool(Walk, true);

            TimeStuck = 0;
        }

        public void Tick()
        {
            if (Vector3.Distance(_thisTransform.position, _lastPosition) <= 0f)
                TimeStuck += Time.deltaTime;
            _lastPosition = _thisTransform.position;
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;

            _animator.SetBool(Walk, false);
        }

        #endregion
    }
}