#region

using System;
using System.Collections.Generic;
using System.Linq;
using Data.Dynamic;
using Data.Static;
using Services.Watchers.PersistentProgressWatcher;
using UI.MainLocation;
using UnityEngine;

#endregion

namespace Skill
{
    public class SkillsManager : MonoBehaviour, IProgressLoadableWatcher, IProgressSavableWatcher
    {
        [SerializeField] private SkillsBookScreen skillsBookScreen;
        [SerializeField] private SkillStaticData[] skillStaticData;
        private Func<int> _getCountLoot;

        private Action<Dictionary<SkillType, int>, int> _onChangeSkills;

        private Dictionary<SkillType, int> _skillsLevelDictionary;

        private Dictionary<SkillType, SkillStaticData> _skillStaticDataDictionary;

        private Predicate<int> _tryToByASkill;

        public void LoadProgress(Progress progress)
        {
            progress.skillsLevel.DeserializeSkills();

            _skillsLevelDictionary = progress.skillsLevel.skills;

            _onChangeSkills?.Invoke(_skillsLevelDictionary, _getCountLoot?.Invoke() ?? 0);
        }

        public void UpdateProgress(Progress progress)
        {
            progress.skillsLevel.skills = _skillsLevelDictionary;
            progress.skillsLevel.SerializeSkills();
        }

        public void SetUp(Predicate<int> tryToByASkill, Func<int> getCountLoot)
        {
            _tryToByASkill = tryToByASkill;
            _getCountLoot = getCountLoot;
            _skillStaticDataDictionary = skillStaticData.ToDictionary(data => data.SkillType, data => data);

            skillsBookScreen.SetUp(TryIncreaseSkill, ref _onChangeSkills, _skillStaticDataDictionary);
        }

        private bool TryIncreaseSkill(SkillType skillType)
        {
            var level = _skillsLevelDictionary[skillType];
            var price = _skillStaticDataDictionary[skillType].GetPriceForLevel(level);
            if (_tryToByASkill?.Invoke(-price) ?? false)
            {
                _skillsLevelDictionary[skillType]++;
                _onChangeSkills?.Invoke(_skillsLevelDictionary, _getCountLoot?.Invoke() ?? 0);

                return true;
            }

            return false;
        }
    }
}
