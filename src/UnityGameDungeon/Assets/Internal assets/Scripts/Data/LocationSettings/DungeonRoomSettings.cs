#region

using UnityEngine;

#endregion

namespace Data.LocationSettings
{
    [CreateAssetMenu(fileName = "DungeonRoomSettings", menuName = "Settings/DungeonRoomSettings", order = 0)]
    public class DungeonRoomSettings : LocationSettings
    {
        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 SocketForWeaponSpawnPosition { get; private set; }
    }
}
