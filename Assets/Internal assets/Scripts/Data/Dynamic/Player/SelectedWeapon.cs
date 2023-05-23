#region

using System;
using Item.Weapon;

#endregion

namespace Data.Dynamic.Player
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
