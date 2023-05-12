using Data.Dynamic.Player;
using UnityEngine;

namespace Data.Static
{
    [CreateAssetMenu(fileName = "SkillStatic", menuName = "StaticData/Skill", order = 0)]
    public class SkillStaticData : ScriptableObject
    {
        [SerializeField] private string nameSkill;
        [SerializeField] private string descriptionSkill;
        [SerializeField] private int valueBuff;
        [SerializeField] private SkillsLevel.SkillsType skillsType;
        

        public string NameSkill => nameSkill;
        public string DescriptionSkill => descriptionSkill;
        public int ValueBuff => valueBuff;
        public SkillsLevel.SkillsType SkillsType => skillsType;

        [Space] [Header("PRICE(x) = a1*((a2*x)^3) + b1*((b2*x)^2) + c*x + d")] 
        [SerializeField] private int a1;
        [SerializeField] private int a2;
        [SerializeField] private int b1;
        [SerializeField] private int b2;
        [SerializeField] private int c;
        [SerializeField] private int d;
        public int GetPriceForLevel(int level) => a1 * ((a2 * level) ^ 3) + b1 * ((b2 * level) ^ 2) + c * level + d;
    }
}