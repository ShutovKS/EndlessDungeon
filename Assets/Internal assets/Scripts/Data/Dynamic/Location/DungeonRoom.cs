using System;
using UnityEngine.Serialization;

namespace Data.Dynamic.Location
{
    [Serializable]
    public class DungeonRoom
    {
        public DungeonRoom()
        {
            roomPassedCount = 0;
            seed = 0;
        }
        
        public int roomPassedCount;
        public int seed;

    }
}