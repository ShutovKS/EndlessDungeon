using UnityEngine;

namespace Data.Static
{
    [CreateAssetMenu(fileName = "PlayerStaticDefaultData", menuName = "StaticData/Player")]
    public class PlayerStaticDefaultData : ScriptableObject
    {
        [field: SerializeField] public float MaxHealthPoints { get; private set; }
        [field: SerializeField] public float DamagePoints { get; private set; }
    }
}