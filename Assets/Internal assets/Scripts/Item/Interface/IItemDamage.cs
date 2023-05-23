namespace Item
{
    public interface IItemDamage
    {
        float Damage { get; }
        bool IsDamage { get; }

        void SetDamage(float value);
        void ItemIsDamage(bool value);
    }
}
