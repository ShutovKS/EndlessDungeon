using System;
using Data.Dynamic.Location;
using Data.Dynamic.Loot;
using Data.Dynamic.Player;

namespace Data.Dynamic
{
    [Serializable]
    public class Progress
    {
        public Progress()
        {
            currentLocation = new CurrentLocation();
            selectedWeapon = new SelectedWeapon();
            dungeonRoom = new DungeonRoom();
            skillsLevel = new SkillsLevel();
            lootData = new LootData();
        }

        public CurrentLocation currentLocation;
        public SelectedWeapon selectedWeapon;
        public DungeonRoom dungeonRoom;
        public SkillsLevel skillsLevel;
        public LootData lootData;
    }
}