using UnityEngine;

namespace Data.Settings
{
    [CreateAssetMenu(fileName = "DungeonRoomSettings", menuName = "Settings/DungeonRoomSettings", order = 0)]
    public class DungeonRoomSettings : BaseSettings
    {
        [field: Space(5f), SerializeField] public Vector3 SocketForWeaponSpawnPosition { get; private set; }
    }
}