using UnityEngine;
using UnityEngine.AI;

namespace Units.Enemy.State_Machines.State
{
    public class MoveToPlayer : IState
    {
        #region Variables

        public float TimeStuck;

        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _thisTransform;
        private readonly Transform _playerTransform;
        private readonly float _speedMove;
        private readonly float _stoppingDistance;
        private Vector3 _lastPosition = Vector3.zero;
        private static readonly int WALK = Animator.StringToHash("Walk");

        #endregion

        #region Constructors

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

        #endregion

        #region Methods

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _speedMove;
            _navMeshAgent.stoppingDistance = _stoppingDistance;

            _animator.SetBool(WALK, true);
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

            _animator.SetBool(WALK, false);
        }

        #endregion
    }
}