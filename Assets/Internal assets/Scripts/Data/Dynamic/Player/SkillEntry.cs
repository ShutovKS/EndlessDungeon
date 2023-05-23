using System;
using Skill;

namespace Data.Dynamic.Player
{
    [Serializable]
    public class SkillEntry
    {
        public SkillType Key;
        public int Value;

        public SkillEntry(SkillType key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}