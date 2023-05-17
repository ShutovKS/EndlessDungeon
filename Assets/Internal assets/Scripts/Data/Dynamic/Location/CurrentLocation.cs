using System;

namespace Data.Dynamic.Location
{
    [Serializable]
    public class CurrentLocation
    {
        public CurrentLocation()
        {
            locationType = LocationType.Main;
        }
        
        public LocationType locationType;

        public enum LocationType
        {
            Main = 0,
            DungeonRoom = 1,
            DungeonBoss = 2,
        }
    }
}