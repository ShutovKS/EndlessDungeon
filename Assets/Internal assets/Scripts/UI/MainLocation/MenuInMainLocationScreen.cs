using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainLocation
{
    public class MenuInMainLocationScreen : MonoBehaviour
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button exitButton;

        public void SetUp(ISaveLoadService saveLoadService)
        {
            saveButton.onClick.AddListener(saveLoadService.SaveProgress);
            exitButton.onClick.AddListener(Application.Quit);
        }
    }
}