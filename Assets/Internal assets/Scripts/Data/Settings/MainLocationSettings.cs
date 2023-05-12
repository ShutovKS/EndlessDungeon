using UnityEngine;

namespace Data.Settings
{
    [CreateAssetMenu(fileName = "MainLocationSettings", menuName = "Settings/MainLocationSettings")]
    public class MainLocationSettings : BaseSettings
    {
        [SerializeField] private Vector3 playerSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3[] weaponSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 socketForWeaponSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 portalSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 portalSpawnRotation;
        [Space(5f)] [SerializeField] private Vector3 skillsBookScreenPosition;
        [Space(5f)] [SerializeField] private Vector3 skillsBookScreenRotation;
        public Vector3 PlayerSpawnPosition => playerSpawnPosition;
        public Vector3[] WeaponSpawnPosition => weaponSpawnPosition;
        public Vector3 SocketForWeaponSpawnPosition => socketForWeaponSpawnPosition;
        public Vector3 PortalSpawnPosition => portalSpawnPosition;
        public Vector3 PortalSpawnRotation => portalSpawnRotation;
        public Vector3 SkillsBookScreenPosition => skillsBookScreenPosition;
        public Vector3 SkillsBookScreenRotation => skillsBookScreenRotation;
    }
}