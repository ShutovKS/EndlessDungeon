#region

using System;
using UnityEngine.Serialization;

#endregion

namespace Data.Dynamic.Loot
{
    [Serializable]
    public class LootData
    {
        public LootData()
        {
            loot = 0;
        }

        public int loot;
    }
}
