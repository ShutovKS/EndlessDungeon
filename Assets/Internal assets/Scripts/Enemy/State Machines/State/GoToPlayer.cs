using UnityEngine;
using UnityEngine.AI;

namespace Enemy.State_Machines.State
{
    public class GoToPlayer : IState
    {
        #region Variables

        public float TimeStuck;

        private readonly Animator _animator;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _thisTransform;
        private readonly Transform _playerTransform;
        private Vector3 _lastPosition = Vector3.zero;
        private static readonly int WALK = Animator.StringToHash("Walk");

        #endregion

        #region Constructors

        public GoToPlayer(Animator animator, NavMeshAgent navMeshAgent, Transform thisTransform,
            Transform playerTransform)
        {
            _animator = animator;
            _navMeshAgent = navMeshAgent;
            _thisTransform = thisTransform;
            _playerTransform = playerTransform;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = 6;
            _navMeshAgent.stoppingDistance = 2f;

            _animator.SetBool(WALK, true);
        }

        public void Tick()
        {
            if (Vector3.Distance(_thisTransform.position, _lastPosition) <= 0f)
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