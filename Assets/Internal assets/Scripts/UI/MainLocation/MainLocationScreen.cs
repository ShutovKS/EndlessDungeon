using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainLocation
{
    public class MainLocationScreen : MonoBehaviour
    {
        [SerializeField] private Button saveButton;

        public void SetUp(ISaveLoadService saveLoadService)
        {
            saveButton.onClick.AddListener(() =>
            {
                saveLoadService.SaveProgress();
            });
        }
    }
}