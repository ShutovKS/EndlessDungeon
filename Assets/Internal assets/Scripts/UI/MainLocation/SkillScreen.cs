#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.MainLocation
{
    public class SkillScreen : MonoBehaviour
    {
        [field: SerializeField] public Image BackgroundSkill { get; private set; }
        [field: SerializeField] public Text NameSkill { get; private set; }
        [field: SerializeField] public Text LevelSkill { get; private set; }
        [field: SerializeField] public Text ValueSkill { get; private set; }
        [field: SerializeField] public Text TypeSkill { get; private set; }
        [field: SerializeField] public Text PriceSkill { get; private set; }
        [field: SerializeField] public Text DescriptionSkill { get; private set; }
        [field: SerializeField] public Button PriceSkillButton { get; private set; }
    }
}
