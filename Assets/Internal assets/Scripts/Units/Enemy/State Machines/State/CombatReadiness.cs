#region

using UnityEngine;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class CombatReadiness : IState
    {
        public CombatReadiness(Transform transform, Transform playerTransform, float speedRotation)
        {
            _transform = transform;
            _playerTransform = playerTransform;
            _speedRotation = speedRotation;
        }

        private readonly Transform _playerTransform;
        private readonly float _speedRotation;
        private readonly Transform _transform;
        public float TimePassed;

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
    }
}
