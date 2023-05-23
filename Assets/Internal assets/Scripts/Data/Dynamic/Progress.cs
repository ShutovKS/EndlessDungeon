#region

using System;
using Data.Dynamic.Location;
using Data.Dynamic.Loot;
using Data.Dynamic.Player;

#endregion

namespace Data.Dynamic
{
    [Serializable]
    public class Progress
    {
        public Progress()
        {
            currentLocation = new CurrentLocation();
            selectedWeapon = new SelectedWeapon();
            dungeonLocation = new DungeonLocation();
            skillsLevel = new SkillsLevel();
            lootData = new LootData();
        }

        public CurrentLocation currentLocation;
        public SelectedWeapon selectedWeapon;
        public DungeonLocation dungeonLocation;
        public SkillsLevel skillsLevel;
        public LootData lootData;
    }
}
