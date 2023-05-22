using UnityEngine;

namespace Socket.SocketInPlayer
{
    public class RotationForwardPlayer : MonoBehaviour
    {
        [SerializeField] private Transform _triggerTransform;

        private void Update()
        {
            var rotation = _triggerTransform.rotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
        }
    }
}