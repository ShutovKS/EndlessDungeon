#region

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#endregion

namespace UI.DungeonRoom
{
    public class MenuInDungeonLocationScreen : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        public void SetUp(UnityAction exitInMainLocation)
        {
            exitButton.onClick.AddListener(exitInMainLocation);
        }
    }
}
