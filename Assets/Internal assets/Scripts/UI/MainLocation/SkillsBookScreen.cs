using System;
using System.Collections.Generic;
using System.Linq;
using Data.Dynamic.Player;
using Data.Static;
using Skill;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.MainLocation
{
    public class SkillsBookScreen : MonoBehaviour
    {
        [SerializeField] private SkillScreen[] skillScreens;

        private Dictionary<SkillType, SkillScreen> _skillScreenDictionary;
        private Dictionary<SkillType, SkillStaticData> _skillStaticDataDictionary;

        private SkillsBook _skillsBook;

        public void SetUp(SkillsBook skillsBook, Dictionary<SkillType, SkillStaticData> skillStaticDatas)
        {
            _skillsBook = skillsBook;
            _skillStaticDataDictionary = skillStaticDatas;
            _skillScreenDictionary = new Dictionary<SkillType, SkillScreen>();

            for (var i = 0; i < skillScreens.Length; i++)
            {
                var skillsType = (SkillType)Enum.GetValues(typeof(SkillType)).GetValue(i);
                _skillScreenDictionary.Add(skillsType, skillScreens[i]);
                SetUISkill(skillsType);
            }

            _skillsBook.RegisterOnChangeSkill(UpdateUISkill);
        }

        private void UpdateUISkill(Dictionary<SkillType, int> levelDictionary, int amountMoney)
        {
            Debug.Log("UpdateUISkill");
            foreach (var (skillType, skillScreen) in _skillScreenDictionary)
            {
                var level = levelDictionary[skillType];
                skillScreen.LevelSkill.text = level.ToString();
                skillScreen.PriceSkill.text = _skillStaticDataDictionary[skillType].GetPriceForLevel(level).ToString();
                skillScreen.PriceSkill.text = (_skillStaticDataDictionary[skillType].BaseValueSkill * level).ToString();
                skillScreen.BackgroundSkill.color = _skillStaticDataDictionary[skillType].GetPriceForLevel(level) > amountMoney ? new Color(1f, 1f, 1f, 0.2f) : new Color(0f, 0f, 0f, 0.2f);
            }
        }

        private void SetUISkill(SkillType skillType)
        {
            Debug.Log($"SetUISkill {skillType.ToString()}");
            _skillScreenDictionary[skillType].NameSkill.text = _skillStaticDataDictionary[skillType].NameSkill;
            _skillScreenDictionary[skillType].DescriptionSkill.text =
                _skillStaticDataDictionary[skillType].DescriptionSkill;

            _skillScreenDictionary[skillType].TypeSkill.text = _skillStaticDataDictionary[skillType].TypeSkill;
            _skillScreenDictionary[skillType].PriceSkillButton.onClick
                .AddListener(() => _skillsBook.TryIncreaseSkill(skillType));

            _skillScreenDictionary[skillType].PriceSkillButton.onClick
                .AddListener(() => Debug.Log($"Нажата кнопка {skillType.ToString()}"));
        }
    }
}