#region

using Skill;
using UnityEngine;

#endregion

namespace Data.Static
{
    [CreateAssetMenu(fileName = "SkillStatic", menuName = "StaticData/Skill", order = 0)]
    public class SkillStaticData : StaticData
    {
        [field: SerializeField] public SkillType SkillType { get; private set; }
        [field: SerializeField] public string NameSkill { get; private set; }
        [field: SerializeField] public int BaseValueSkill { get; private set; }
        [field: SerializeField] public string TypeSkill { get; private set; }
        [field: SerializeField] public string DescriptionSkill { get; private set; }

        [Space] [Header("PRICE(x) = a1*((a2*x)^3) + b1*((b2*x)^2) + c*x + d")] [SerializeField]
        private int a1;

        [SerializeField] private int a2;
        [SerializeField] private int b1;
        [SerializeField] private int b2;
        [SerializeField] private int c;
        [SerializeField] private int d;

        public int GetPriceForLevel(int level)
        {
            return a1 * (a2 * level ^ 3) + b1 * (b2 * level ^ 2) + c * level + d;
        }
    }
}
