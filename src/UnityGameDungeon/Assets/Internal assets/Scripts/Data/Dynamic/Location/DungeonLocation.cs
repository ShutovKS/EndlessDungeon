#region

using System;

#endregion

namespace Data.Dynamic.Location
{
    [Serializable]
    public class DungeonLocation
    {
        public DungeonLocation()
        {
            roomPassedCount = 0;
            seed = 0;
        }

        public int roomPassedCount;
        public int seed;
    }
}
