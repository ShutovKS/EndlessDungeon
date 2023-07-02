#region

using UnityEngine;

#endregion

namespace Data.LocationSettings
{
    [CreateAssetMenu(fileName = "MainLocationSettings", menuName = "Settings/MainLocationSettings")]
    public class MainLocationSettings : LocationSettings
    {
        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 PlayerSpawnPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3[] WeaponSpawnPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 SocketForWeaponSpawnPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 TransitionToTheDungeonSpawnPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 SkillsBookScreenPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 SkillsBookScreenRotation { get; private set; }
    }
}
