#region

using UnityEngine;

#endregion

namespace Socket
{
    public class FollowThePlayer : MonoBehaviour
    {
        [SerializeField] private Transform triggerTransform;

        private void LateUpdate()
        {
            var rotation = triggerTransform.rotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, 0));
        }
        
        
    }
}
