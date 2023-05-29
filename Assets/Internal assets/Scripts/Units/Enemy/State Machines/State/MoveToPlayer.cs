#region

using UnityEngine;
using UnityEngine.AI;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class MoveToPlayer : IState
    {
        public MoveToPlayer(Animator animator, NavMeshAgent navMeshAgent, Transform thisTransform,
            Transform playerTransform, float speedMove, float stoppingDistance)
        {
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _thisTransform = thisTransform;
            _playerTransform = playerTransform;
            _speedMove = speedMove;
            _stoppingDistance = stoppingDistance;
        }
        private readonly static int Walk = Animator.StringToHash("Walk");

        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _playerTransform;
        private readonly float _speedMove;
        private readonly float _stoppingDistance;
        private readonly Transform _thisTransform;
        private Vector3 _lastPosition = Vector3.zero;
        public float TimeStuck;

        public void OnEnter()
        {
            TimeStuck = 0;
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _speedMove;
            _navMeshAgent.stoppingDistance = _stoppingDistance;

            _animator.SetBool(Walk, true);
        }

        public void Tick()
        {
            if (Vector3.Distance(_thisTransform.position, _lastPosition) <= 0.025f)
                TimeStuck += Time.deltaTime;

            _navMeshAgent.SetDestination(_playerTransform.position);
            _lastPosition = _thisTransform.position;
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
            _animator.SetBool(Walk, false);
        }
    }
}
