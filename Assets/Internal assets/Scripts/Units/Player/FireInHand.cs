using UnityEngine;

namespace Units.Player
{
    public class FireInHand : MonoBehaviour
    {
        private GameObject _fire;
        private Transform _transform;
        private bool _state;

        private void Start()
        {
            _transform = transform.parent;
            _fire = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            var rotation = _transform.rotation.eulerAngles;
            _state = rotation.z is > 75 and < 135 && rotation.x is > 330 or < 30;
            _fire.SetActive(_state);
        }
    }
}