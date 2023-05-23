#region

using UnityEngine;
using UnityEngine.AI;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class Patrol : IState
    {
        public Patrol(Animator animator, NavMeshAgent navMeshAgent, Transform thisTransform, Transform targetTransform,
            float speed)
        {
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _thisTransform = thisTransform;
            _targetTransform = targetTransform;
            _speed = speed;
        }

        private readonly static int Walk = Animator.StringToHash("Walk");

        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _speed;
        private readonly Transform _targetTransform;
        private readonly Transform _thisTransform;
        private Vector3 _lastPosition = Vector3.zero;
        public float TimeStuck;

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _speed;
            _navMeshAgent.stoppingDistance = 0f;
            _navMeshAgent.SetDestination(_targetTransform.position);
            _animator.SetBool(Walk, true);
            TimeStuck = 0;
        }

        public void Tick()
        {
            if (Vector3.Distance(_thisTransform.position, _lastPosition) <= 0.025f)
                TimeStuck += Time.deltaTime;

            _lastPosition = _thisTransform.position;
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
            _animator.SetBool(Walk, false);
        }
    }
}
