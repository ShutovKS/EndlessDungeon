using UnityEngine;

namespace Item
{
    public interface IItemDamage
    {
        bool IsDamage { get; set; }
        float Damage { get; set; }

        void ItemIsDamage(bool value);
    }
}