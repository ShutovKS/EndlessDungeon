using System;
using Data.Dynamic.Loot;
using Data.Dynamic.Player;

namespace Data.Dynamic
{
    [Serializable]
    public class Progress
    {
        public Progress()
        {
            selectedWeapon = new SelectedWeapon();
            lootData = new LootData();
            skillsLevel = new SkillsLevel();
        }

        public SelectedWeapon selectedWeapon;
        public SkillsLevel skillsLevel;
        public LootData lootData;
    }
}