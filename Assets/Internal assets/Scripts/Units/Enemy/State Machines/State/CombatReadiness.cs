using UnityEngine;

namespace Units.Enemy.State_Machines.State
{
    public class CombatReadiness : IState
    {
        #region Variables

        public float TimePassed;
        private readonly Animator _animator;
        private Transform _playerTransform;
        private Transform _transform;
        private float _speedRotation;

        #endregion

        #region Constructors

        public CombatReadiness(Animator animator, Transform transform, Transform playerTransform, float speedRotation)
        {
            _animator = animator;
            _transform = transform;
            _playerTransform = playerTransform;
            _speedRotation = speedRotation;
        }

        #endregion

        #region Methods

        public void OnEnter()
        {
            TimePassed = 0f;
        }

        public void Tick()
        {
            TimePassed += Time.deltaTime;

            var rotation = _transform.rotation;
            var positionDifference = _playerTransform.position - _transform.position;
            var positionToLook = new Vector3(positionDifference.x, 0, positionDifference.z);

            rotation = Quaternion.Lerp(
                rotation,
                Quaternion.LookRotation(positionToLook),
                _speedRotation * Time.deltaTime);

            _transform.rotation = rotation;
        }

        public void OnExit()
        {
        }

        #endregion

        #region Methods Other

        #endregion
    }
}