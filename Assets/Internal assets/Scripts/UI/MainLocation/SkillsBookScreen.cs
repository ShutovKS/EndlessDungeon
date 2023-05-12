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

        private readonly Dictionary<SkillsLevel.SkillsType, UISkill> _uiSkillsDictionary = new()
        {
            { SkillsLevel.SkillsType.STREANGHT_Count, null },
            { SkillsLevel.SkillsType.STREANGHT_Percent, null },
            { SkillsLevel.SkillsType.PROCTION_Count, null },
            { SkillsLevel.SkillsType.PROCTION_Percent, null },
            { SkillsLevel.SkillsType.HEALTH_Count, null },
            { SkillsLevel.SkillsType.HEALTH_Percent, null },
        };

        private SkillsBook _skillsBook;

        public void SetUp(SkillsBook skillsBook, Dictionary<SkillsLevel.SkillsType, SkillStaticData> skillStaticDatas)
        {
            _skillsBook = skillsBook;

            for (var i = 0; i < _uiSkillsDictionary.Count; i++)
            {
                var skillsType = (SkillsLevel.SkillsType)Enum.GetValues(typeof(SkillsLevel.SkillsType)).GetValue(i);

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

        private void UpdateUISkill(SkillsLevel.SkillsType skillsType, int level)
        {
            Debug.Log($"Update {skillsType}");
            
            _uiSkillsDictionary[skillsType].descriptionSkill.text =
                $"{_uiSkillsDictionary[skillsType].skillStaticData.DescriptionSkill ?? "No description"}\n" +
                $"{level}";
        }
        
        private void SetUISkill(SkillsLevel.SkillsType skillsType)
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