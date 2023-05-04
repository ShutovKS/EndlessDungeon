using System;
using Item.Weapon;
using UnityEngine.Events;

namespace Data.Dynamic.PlayerData
{
    [Serializable]
    public class SelectedWeapon
    {
        public SelectedWeapon()
        {
            weaponType = WeaponType.Sword;
        }
        
        public WeaponType weaponType;
    }
}