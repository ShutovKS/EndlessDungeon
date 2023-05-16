using System;
using System.Collections.Generic;
using System.Linq;
using Skill;

namespace Data.Dynamic.Player
{
    [Serializable]
    public class SkillsLevel
    {
        public SkillsLevel()
        {
            Skills = new Dictionary<SkillType, int>
            {
                { SkillType.STREANGHT_Count, 0 },
                { SkillType.STREANGHT_Percent, 0 },
                { SkillType.PROTECTION_Count, 0 },
                { SkillType.PROTECTION_Percent, 0 },
                { SkillType.HEALTH_Count, 0 },
                { SkillType.HEALTH_Percent, 0 },
            };
        }

        public Dictionary<SkillType, int> Skills;

        public List<SkillEntry> SkillEntries;

        public void SerializeSkills()
        {
            SkillEntries = Skills.Select(pair => new SkillEntry(pair.Key, pair.Value)).ToList();
        }

        public void DeserializeSkills()
        {
            if (SkillEntries == null) return;
            var skills = SkillEntries.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var (skillsType, level) in skills)
                Skills[skillsType] = level;
        }
    }

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