using UnityEngine;

namespace Data.Settings
{
    [CreateAssetMenu(fileName = "MainLocationSettings", menuName = "Settings/MainLocationSettings")]
    public class MainLocationSettings : BaseSettings
    {
        [SerializeField] private Vector3 _playerSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3[] _weaponSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 socketForWeaponSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 portalSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 portalSpawnRotation;
        public Vector3 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector3[] WeaponSpawnPosition => _weaponSpawnPosition;
        public Vector3 SocketForWeaponSpawnPosition => socketForWeaponSpawnPosition;
        public Vector3 PortalSpawnPosition => portalSpawnPosition;
        public Vector3 PortalSpawnRotation => portalSpawnRotation;
    }
}