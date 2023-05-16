using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainLocation
{
    public class SkillScreen : MonoBehaviour
    {
        [field: SerializeField] public Image BackgroundSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI NameSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI LevelSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI ValueSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI TypeSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI PriceSkill {get; private set;}
        [field: SerializeField] public TextMeshProUGUI DescriptionSkill {get; private set;}
        [field: SerializeField] public Button PriceSkillButton { get; private set; }
    }
}