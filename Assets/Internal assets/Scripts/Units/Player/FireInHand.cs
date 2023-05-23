#region

using UnityEngine;

#endregion

namespace Units.Player
{
    public class FireInHand : MonoBehaviour
    {
        private GameObject _fire;
        private Transform _transform;

        private void Start()
        {
            _transform = transform.parent;
            _fire = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            var rotation = _transform.rotation;
            var rotationEulerAngles = rotation.eulerAngles;
            var state = rotationEulerAngles.z is > 75 and < 135 && rotationEulerAngles.x is > 330 or < 30;
            _fire.SetActive(state);
        }
    }
}
