#region

using UnityEngine;

#endregion

namespace BuildSetting
{
    public class GameObjectDisable : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
#else
            Destroy(gameObject);
#endif
        }
    }
}
