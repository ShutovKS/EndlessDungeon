#region

using System;
using System.Collections.Generic;
using Data.Static;
using Skill;
using UnityEngine;

#endregion

namespace UI.MainLocation
{
    public class SkillsBookScreen : MonoBehaviour
    {
        [SerializeField] private SkillScreen[] skillScreens;

        private Predicate<SkillType> _onTryIncreaseSkill;

        private Dictionary<SkillType, SkillScreen> _skillScreenDictionary;
        private Dictionary<SkillType, SkillStaticData> _skillStaticDataDictionary;

        public void SetUp(Predicate<SkillType> onTryIncreaseSkill,
            ref Action<Dictionary<SkillType, int>, int> onChangeSkills,
            Dictionary<SkillType, SkillStaticData> skillStaticDictionary)
        {
            _skillStaticDataDictionary = skillStaticDictionary;
            _skillScreenDictionary = new Dictionary<SkillType, SkillScreen>();

            for (var i = 0; i < skillScreens.Length; i++)
            {
                var skillsType = (SkillType)Enum.GetValues(typeof(SkillType)).GetValue(i);
                _skillScreenDictionary.Add(skillsType, skillScreens[i]);
                SetUISkill(skillsType);
            }

            _onTryIncreaseSkill = onTryIncreaseSkill;
            onChangeSkills += UpdateAllUISkills;
        }

        private void UpdateAllUISkills(Dictionary<SkillType, int> levelDictionary, int amountMoney)
        {
            foreach (var (skillType, skillLevel) in levelDictionary)
            {
                UpdateUISkill(skillType, skillLevel, amountMoney);
            }
        }

        private void UpdateUISkill(SkillType skillType, int skillLevel, int amountMoney)
        {
            if (!_skillScreenDictionary.TryGetValue(skillType, out var skillScreen) ||
                !_skillStaticDataDictionary.TryGetValue(skillType, out var skillStaticData)) return;

            skillScreen.LevelSkill.text = $"{skillLevel}";
            skillScreen.PriceSkill.text = $"{skillStaticData.GetPriceForLevel(skillLevel)}";
            skillScreen.ValueSkill.text = $"{skillStaticData.BaseValueSkill * skillLevel}";
            skillScreen.BackgroundSkill.color =
                skillStaticData.GetPriceForLevel(skillLevel) > amountMoney
                    ? new Color(1f, 1f, 1f, 0.2f)
                    : new Color(0f, 0f, 0f, 0.2f);
        }

        private void SetUISkill(SkillType skillType)
        {
            if (!_skillScreenDictionary.TryGetValue(skillType, out var skillScreen) ||
                !_skillStaticDataDictionary.TryGetValue(skillType, out var skillStaticData)) return;

            skillScreen.PriceSkillButton.onClick.AddListener(() => _onTryIncreaseSkill?.Invoke(skillType));
            skillScreen.DescriptionSkill.text = skillStaticData.DescriptionSkill;
            skillScreen.NameSkill.text = skillStaticData.NameSkill;
            skillScreen.TypeSkill.text = skillStaticData.TypeSkill;

        }
    }
}
