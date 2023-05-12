using System;
using System.Collections.Generic;

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
                { SkillsType.PROCTION_Count, 0 },
                { SkillsType.PROCTION_Percent, 0 },
                { SkillsType.HEALTH_Count, 0 },
                { SkillsType.HEALTH_Percent, 0 },
            };
        }

        public Dictionary<SkillsType, int> Skills;

        public enum SkillsType
        {
            STREANGHT_Count = 0,
            STREANGHT_Percent = 1,
            PROCTION_Count = 2,
            PROCTION_Percent = 3,
            HEALTH_Count = 4,
            HEALTH_Percent = 5,
        }
    }
}