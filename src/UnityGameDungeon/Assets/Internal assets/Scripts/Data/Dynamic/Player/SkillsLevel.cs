#region

using System;
using System.Collections.Generic;
using System.Linq;
using Skill;
using UnityEngine.Serialization;

#endregion

namespace Data.Dynamic.Player
{
    [Serializable]
    public class SkillsLevel
    {
        public SkillsLevel()
        {
            skills = new Dictionary<SkillType, int>
            {
                [SkillType.StrengthCount] = 0,
                [SkillType.StrengthPercent] = 0,
                [SkillType.ProtectionCount] = 0,
                [SkillType.ProtectionPercent] = 0,
                [SkillType.HealthCount] = 0,
                [SkillType.HealthPercent] = 0
            };
        }

        public List<SkillEntry> skillEntries;

        public Dictionary<SkillType, int> skills;

        public void SerializeSkills()
        {
            skillEntries = skills.Select(pair => new SkillEntry(pair.Key, pair.Value)).ToList();
        }

        public void DeserializeSkills()
        {
            if (skillEntries == null) return;
            skills = skillEntries.ToDictionary(entry => entry.key, entry => entry.value);
        }
    }
}
