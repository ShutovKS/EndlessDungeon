#region

using UnityEngine;

#endregion

namespace Units.Enemy.State_Machines.State
{
    public class SearchPositionForPatrol : IState
    {
        public SearchPositionForPatrol(Transform thisTransform, out Transform targetTransform)
        {
            _thisTransform = thisTransform;
            _targetTransform = new GameObject().transform;
            targetTransform = _targetTransform;
        }
        private readonly Transform _targetTransform;
        private readonly Transform _thisTransform;

        public void OnEnter()
        {
        }

        public void Tick()
        {
            _targetTransform.position = NewPositionPatrol(_thisTransform);
        }

        public void OnExit()
        {
        }

        private static Vector3 NewPositionPatrol(Transform transform)
        {
            return transform.position +
                transform.forward * (Random.Range(15f, 30f) * Random.Range(-1, 2)) +
                new Vector3(Random.Range(10f, 25f) * Random.Range(-1, 2), 0, 0);
        }
    }
}
