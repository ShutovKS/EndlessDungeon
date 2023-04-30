using UnityEngine;
using UnityEngine.AI;

namespace Enemy.State_Machines.State
{
    public class SearchPositionForPatrol : IState
    {
        #region Variables

        private readonly Transform _thisTransform;
        private readonly Transform _targetTransform;

        #endregion

        #region Constructors

        public SearchPositionForPatrol(Transform thisTransform, out Transform targetTransform)
        {
            _thisTransform = thisTransform;

            _targetTransform = new GameObject().transform;
            targetTransform = _targetTransform;
        }

        #endregion

        #region Methods

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

        #endregion

        #region Methods Other

        private static Vector3 NewPositionPatrol(Transform transform)
        {
            return transform.position +
                   transform.forward * (Random.Range(10f, 20f) * Random.Range(-1, 2)) +
                   new Vector3(Random.Range(7.5f, 15f) * Random.Range(-1, 2), 0, 0);
        }

        #endregion
    }
}