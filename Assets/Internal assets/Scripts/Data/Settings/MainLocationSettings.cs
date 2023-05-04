using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Settings
{
    [CreateAssetMenu(fileName = "MainLocationSettings", menuName = "Settings/MainLocationSettings")]
    public class MainLocationSettings : BaseSettings
    {
        [SerializeField] private Vector3 _playerSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3[] _weaponSpawnPosition;
        [Space(5f)] [SerializeField] private Vector3 socketForWeaponSpawnPosition;
 
        public Vector3 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector3[] WeaponSpawnPosition => _weaponSpawnPosition;
        public Vector3 SocketForWeaponSpawnPosition => socketForWeaponSpawnPosition;
    }
}