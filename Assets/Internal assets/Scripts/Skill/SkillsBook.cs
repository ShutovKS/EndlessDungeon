using System;
using System.Collections.Generic;
using Data.Dynamic;
using Data.Dynamic.Player;
using Data.Static;
using Loot;
using Services.PersistentProgress;
using UI.MainLocation;
using UnityEngine;
using UnityEngine.UI;

namespace Skill
{
    public class SkillsBook : MonoBehaviour, IProgressLoadable, IProgressSavable
    {
        [SerializeField] private SkillsBookScreen skillsBookScreen;
        [SerializeField] private SkillStaticData[] skillStaticDatas;

        private readonly Dictionary<SkillsType, int> _skillsLevel = new()
        {
            [SkillsType.STREANGHT_Count] = 0,
            [SkillsType.STREANGHT_Percent] = 0,
            [SkillsType.PROTECTION_Count] = 0,
            [SkillsType.PROTECTION_Percent] = 0,
            [SkillsType.HEALTH_Count] = 0,
            [SkillsType.HEALTH_Percent] = 0,
        };

        private readonly Dictionary<SkillsType, SkillStaticData> _skillStaticDatas = new()
        {
            [SkillsType.STREANGHT_Count] = null,
            [SkillsType.STREANGHT_Percent] = null,
            [SkillsType.PROTECTION_Count] = null,
            [SkillsType.PROTECTION_Percent] = null,
            [SkillsType.HEALTH_Count] = null,
            [SkillsType.HEALTH_Percent] = null,
        };

        private Action<SkillsType, int> _onChangeSkills;
        private LootManager _lootManager;

        public void SetUp(LootManager lootManager)
        {
            _lootManager = lootManager;

            foreach (var data in skillStaticDatas)
                _skillStaticDatas[data.SkillsType] = data;

            skillsBookScreen.SetUp(this, _skillStaticDatas);
        }

        public void TryIncreaseSkill(SkillsType skillsType)
        {
            var level = _skillsLevel[skillsType];
            var price = _skillStaticDatas[skillsType].GetPriceForLevel(level);
            if (!_lootManager.TryAmountChangeOnThe(-price)) return;
            _skillsLevel[skillsType]++;
            _onChangeSkills?.Invoke(skillsType, _skillsLevel[skillsType]);
        }

        public void RegisterOnChangeSkill(Action<SkillsType, int> onChangeSkill)
        {
            _onChangeSkills += onChangeSkill;
        }

        public void LoadProgress(Progress progress)
        {
            progress.skillsLevel.DeserializeSkills();
            foreach (var skillsType in (SkillsType[])Enum.GetValues(typeof(SkillsType)))
            {
                _skillsLevel[skillsType] = progress.skillsLevel.Skills[skillsType];
                _onChangeSkills?.Invoke(skillsType, _skillsLevel[skillsType]);
            }
        }

        public void UpdateProgress(Progress progress)
        {
            progress.skillsLevel.Skills = _skillsLevel;
            progress.skillsLevel.SerializeSkills();
        }
    }
}