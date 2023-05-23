#region

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#endregion

namespace UI.MainLocation
{
    public class MenuInMainLocationScreen : MonoBehaviour
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button exitButton;

        public void SetUp(UnityAction saveProgress, UnityAction exitInMainMenu)
        {
            saveButton.onClick.AddListener(saveProgress);
            exitButton.onClick.AddListener(exitInMainMenu);
        }
    }
}
