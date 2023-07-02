#region

using UnityEngine;

#endregion

namespace Data.LocationSettings
{
    [CreateAssetMenu(fileName = "MainMenuSettings", menuName = "Settings/MainMenuSettings")]
    public class MainMenuSettings : LocationSettings
    {
        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 UIMenuSpawnPosition { get; private set; }

        [field: Space(5f)]
        [field: SerializeField]
        public Vector3 PlayerSpawnPosition { get; private set; }
    }
}
