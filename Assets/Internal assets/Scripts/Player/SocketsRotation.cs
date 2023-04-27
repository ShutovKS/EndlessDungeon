using System;
using UnityEngine;

namespace Player
{
    public class SocketsRotation : MonoBehaviour
    {
        private Transform _triggerTransform;

        private void Awake()
        {
            _triggerTransform = Camera.main!.transform;
        }

        private void Update()
        {
            var rotation = _triggerTransform.rotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
        }
    }
}