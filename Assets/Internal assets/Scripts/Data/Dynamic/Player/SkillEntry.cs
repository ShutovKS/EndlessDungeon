#region

using System;
using Skill;

#endregion

namespace Data.Dynamic.Player
{
    [Serializable]
    public class SkillEntry
    {
        public SkillEntry(SkillType key, int value)
        {
            this.key = key;
            this.value = value;
        }

        public SkillType key;
        public int value;
    }
}
