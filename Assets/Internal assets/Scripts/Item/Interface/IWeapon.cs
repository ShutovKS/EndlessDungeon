using Item.Weapon;

namespace Item
{
    public interface IWeapon
    {
        WeaponType WeaponType { get; }

        void SetWeaponTypeInDefault();
    }
}