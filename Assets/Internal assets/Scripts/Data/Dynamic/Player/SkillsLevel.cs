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
            Skills = new Dictionary<SkillsType, int>
            {
                { SkillsType.STREANGHT_Count, 0 },
                { SkillsType.STREANGHT_Percent, 0 },
                { SkillsType.PROTECTION_Count, 0 },
                { SkillsType.PROTECTION_Percent, 0 },
                { SkillsType.HEALTH_Count, 0 },
                { SkillsType.HEALTH_Percent, 0 },
            };
        }

        public Dictionary<SkillsType, int> Skills;

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
        public SkillsType Key;
        public int Value;

        public SkillEntry(SkillsType key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}