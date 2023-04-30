using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class PlayerDetector : MonoBehaviour
    {
        public bool PlayerInRange;

        private void Awake()
        {
            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = 25;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("Player")) PlayerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.CompareTag("Player")) PlayerInRange = false;
        }
    }
}