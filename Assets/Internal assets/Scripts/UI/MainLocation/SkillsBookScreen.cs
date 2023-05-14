using System;
using System.Collections.Generic;
using System.Linq;
using Data.Dynamic.Player;
using Data.Static;
using Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.MainLocation
{
    public class SkillsBookScreen : MonoBehaviour
    {
        [SerializeField] private GameObject[] uiSkills;

        private readonly Dictionary<SkillsType, UISkill> _uiSkillsDictionary = new()
        {
            { SkillsType.STREANGHT_Count, null },
            { SkillsType.STREANGHT_Percent, null },
            { SkillsType.PROTECTION_Count, null },
            { SkillsType.PROTECTION_Percent, null },
            { SkillsType.HEALTH_Count, null },
            { SkillsType.HEALTH_Percent, null },
        };

        private SkillsBook _skillsBook;

        public void SetUp(SkillsBook skillsBook, Dictionary<SkillsType, SkillStaticData> skillStaticDatas)
        {
            _skillsBook = skillsBook;

            for (var i = 0; i < _uiSkillsDictionary.Count; i++)
            {
                var skillsType = (SkillsType)Enum.GetValues(typeof(SkillsType)).GetValue(i);

                _uiSkillsDictionary[skillsType] = new UISkill
                {
                    imagePanelSkill = uiSkills[i].GetComponent<Image>(),
                    imageSkill = uiSkills[i].transform.GetChild(0).GetComponent<Image>(),
                    nameSkill = uiSkills[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>(),
                    descriptionSkill = uiSkills[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>(),
                    priceSkillButton = uiSkills[i].GetComponentInChildren<Button>(),
                    skillStaticData = skillStaticDatas[skillsType]
                };
                SetUISkill(skillsType);
            }
            
            _skillsBook.RegisterOnChangeSkill(UpdateUISkill);
        }

        private void UpdateUISkill(SkillsType skillsType, int level)
        {
            _uiSkillsDictionary[skillsType].descriptionSkill.text =
                $"{_uiSkillsDictionary[skillsType].skillStaticData.DescriptionSkill ?? "No description"}\n" +
                $"{level}";
        }
        
        private void SetUISkill(SkillsType skillsType)
        {
            _uiSkillsDictionary[skillsType].priceSkillButton.onClick.AddListener(() => _skillsBook.TryIncreaseSkill(skillsType));
            _uiSkillsDictionary[skillsType].imageSkill.color = Random.ColorHSV();
            _uiSkillsDictionary[skillsType].nameSkill.text = _uiSkillsDictionary[skillsType].skillStaticData.NameSkill;
            _uiSkillsDictionary[skillsType].descriptionSkill.text =
                $"{_uiSkillsDictionary[skillsType].skillStaticData.DescriptionSkill ?? "No description"}\n" +
                $"{_uiSkillsDictionary[skillsType].skillStaticData.ValueBuff}";
        }

        [Serializable]
        private class UISkill
        {
            public Image imagePanelSkill;
            public Button priceSkillButton;
            public Image imageSkill;
            public TextMeshProUGUI nameSkill;
            public TextMeshProUGUI descriptionSkill;

            public SkillStaticData skillStaticData;
        }
    }
}