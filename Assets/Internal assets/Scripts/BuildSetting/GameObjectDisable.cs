using UnityEngine;

namespace BuildSetting
{
    public class GameObjectDisable : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
#else
            gameObject.SetActive(false);
#endif
        }
    }
}