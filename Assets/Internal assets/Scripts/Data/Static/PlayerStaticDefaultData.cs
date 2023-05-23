#region

using UnityEngine;

#endregion

namespace Data.Static
{
    [CreateAssetMenu(fileName = "PlayerStaticDefaultData", menuName = "StaticData/Player")]
    public class PlayerStaticDefaultData : StaticData
    {
        [field: SerializeField] public float HealthMaxPoints { get; private set; }
        [field: SerializeField] public float DamagePoints { get; private set; }
        [field: SerializeField] public float ProtectionPoints { get; private set; }
    }
}
