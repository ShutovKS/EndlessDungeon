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

        private readonly Dictionary<SkillType, int> _skillsLevelDictionary = new()
        {
            [SkillType.STREANGHT_Count] = 0,
            [SkillType.STREANGHT_Percent] = 0,
            [SkillType.PROTECTION_Count] = 0,
            [SkillType.PROTECTION_Percent] = 0,
            [SkillType.HEALTH_Count] = 0,
            [SkillType.HEALTH_Percent] = 0,
        };

        private readonly Dictionary<SkillType, SkillStaticData> _skillStaticDataDictionary = new()
        {
            [SkillType.STREANGHT_Count] = null,
            [SkillType.STREANGHT_Percent] = null,
            [SkillType.PROTECTION_Count] = null,
            [SkillType.PROTECTION_Percent] = null,
            [SkillType.HEALTH_Count] = null,
            [SkillType.HEALTH_Percent] = null,
        };

        private Action<Dictionary<SkillType, int>, int> _onChangeSkills;
        private LootManager _lootManager;

        public void SetUp(LootManager lootManager)
        {
            _lootManager = lootManager;

            foreach (var data in skillStaticDatas)
                _skillStaticDataDictionary[data.SkillType] = data;

            skillsBookScreen.SetUp(this, _skillStaticDataDictionary);
        }

        public void TryIncreaseSkill(SkillType skillType)
        {
            var level = _skillsLevelDictionary[skillType];
            var price = _skillStaticDataDictionary[skillType].GetPriceForLevel(level);
            if (!_lootManager.TryAmountChangeOnThe(-price)) return;
            _skillsLevelDictionary[skillType]++;
            _onChangeSkills?.Invoke(_skillsLevelDictionary, _lootManager.SoulsOfTheDungeon);
        }

        public void RegisterOnChangeSkill(Action<Dictionary<SkillType, int>, int> onChangeSkill)
        {
            _onChangeSkills += onChangeSkill;
        }

        public void LoadProgress(Progress progress)
        {
            progress.skillsLevel.DeserializeSkills();

            foreach (var (key, level) in progress.skillsLevel.Skills)
                _skillsLevelDictionary[key] = level;

            _onChangeSkills?.Invoke(_skillsLevelDictionary, _lootManager.SoulsOfTheDungeon);
        }

        public void UpdateProgress(Progress progress)
        {
            progress.skillsLevel.Skills = _skillsLevelDictionary;
            progress.skillsLevel.SerializeSkills();
        }
    }
}