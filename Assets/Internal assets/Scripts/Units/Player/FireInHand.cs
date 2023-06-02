#region

using UnityEngine;

#endregion

namespace Units.Player
{
    public class FireInHand : MonoBehaviour
    {
        private GameObject _fire;
        private Transform _transform;
        [SerializeField] private float minAngleZ;
        [SerializeField] private float maxAngleZ;
        [SerializeField] private float minAngleX;
        [SerializeField] private float maxAngleX;

        private void Start()
        {
            _transform = transform.parent;
            _fire = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            var rotation = _transform.rotation;
            var rotationEulerAngles = rotation.eulerAngles;
            var state = rotationEulerAngles.z > minAngleZ && rotationEulerAngles.z < maxAngleZ && (rotationEulerAngles.x > minAngleX || rotationEulerAngles.x < maxAngleX);
            _fire.SetActive(state);
        }
    }
}
