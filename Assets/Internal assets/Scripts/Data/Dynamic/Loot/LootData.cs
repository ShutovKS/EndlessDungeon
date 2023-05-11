using System;

namespace Data.Dynamic.Loot
{
    [Serializable]
    public class LootData
    {
        public event Action IsAmountChanged;
        public int soulsOfTheDungeon;

        public void Collected(int newSoulsOfTheDungeon)
        {
            soulsOfTheDungeon += newSoulsOfTheDungeon;
            IsAmountChanged?.Invoke();
        }
    }
}