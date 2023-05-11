using System;
using Data.Dynamic.Loot;
using Data.Dynamic.Player;
using UnityEngine.Serialization;

namespace Data.Dynamic
{
    [Serializable]
    public class Progress
    {
        public Progress()
        {
            selectedWeapon = new SelectedWeapon();
            lootData = new LootData();
        }

        public SelectedWeapon selectedWeapon;
        public LootData lootData;
    }
}