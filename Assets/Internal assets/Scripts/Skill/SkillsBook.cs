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

        private readonly Dictionary<SkillsLevel.SkillsType, int> _skillsLevel = new()
        {
            [SkillsLevel.SkillsType.STREANGHT_Count] = 0,
            [SkillsLevel.SkillsType.STREANGHT_Percent] = 0,
            [SkillsLevel.SkillsType.PROCTION_Count] = 0,
            [SkillsLevel.SkillsType.PROCTION_Percent] = 0,
            [SkillsLevel.SkillsType.HEALTH_Count] = 0,
            [SkillsLevel.SkillsType.HEALTH_Percent] = 0,
        };

        private readonly Dictionary<SkillsLevel.SkillsType, SkillStaticData> _skillStaticDatas = new()
        {
            [SkillsLevel.SkillsType.STREANGHT_Count] = null,
            [SkillsLevel.SkillsType.STREANGHT_Percent] = null,
            [SkillsLevel.SkillsType.PROCTION_Count] = null,
            [SkillsLevel.SkillsType.PROCTION_Percent] = null,
            [SkillsLevel.SkillsType.HEALTH_Count] = null,
            [SkillsLevel.SkillsType.HEALTH_Percent] = null,
        };

        private Action<SkillsLevel.SkillsType, int> _onChangeSkills;
        private LootManager _lootManager;

        public void SetUp(LootManager lootManager)
        {
            _lootManager = lootManager;

            foreach (var data in skillStaticDatas)
                _skillStaticDatas[data.SkillsType] = data;

            skillsBookScreen.SetUp(this, _skillStaticDatas);
        }

        public void TryIncreaseSkill(SkillsLevel.SkillsType skillsType)
        {
            var level = _skillsLevel[skillsType];
            var price = _skillStaticDatas[skillsType].GetPriceForLevel(level);
            if (!_lootManager.TryAmountChangeOnThe(-price)) return;
            _skillsLevel[skillsType]++;
            _onChangeSkills?.Invoke(skillsType, _skillsLevel[skillsType]);
        }

        public void RegisterOnChangeSkill(Action<SkillsLevel.SkillsType, int> onChangeSkill)
        {
            _onChangeSkills += onChangeSkill;
        }

        public void LoadProgress(Progress progress)
        {
            foreach (var skillsType in (SkillsLevel.SkillsType[])Enum.GetValues(typeof(SkillsLevel.SkillsType)))
            {
                _skillsLevel[skillsType] = progress.skillsLevel.Skills[skillsType];
                _onChangeSkills?.Invoke(skillsType, _skillsLevel[skillsType]);
                Debug.Log(skillsType + "\t" + _skillsLevel[skillsType]);
            }
        }

        public void UpdateProgress(Progress progress)
        {
            progress.skillsLevel.Skills = _skillsLevel;

            foreach (var skillsType in (SkillsLevel.SkillsType[])Enum.GetValues(typeof(SkillsLevel.SkillsType)))
            {
                progress.skillsLevel.Skills[skillsType] = _skillsLevel[skillsType];
                Debug.Log(skillsType + "\t" + progress.skillsLevel.Skills[skillsType]);
            }
        }
    }
}