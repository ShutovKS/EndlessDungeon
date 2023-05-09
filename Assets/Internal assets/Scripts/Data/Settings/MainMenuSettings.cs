using UnityEngine;

namespace Data.Settings
{
    [CreateAssetMenu(fileName = "MainMenuSettings", menuName = "Settings/MainMenuSettings")]
    public class MainMenuSettings : BaseSettings
    {
        [SerializeField] private Vector3 uiMenuSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 playerSpawnPosition;

        public Vector3 UIMenuSpawnPosition => uiMenuSpawnPosition;
        public Vector3 PlayerSpawnPosition => playerSpawnPosition;
    }
}