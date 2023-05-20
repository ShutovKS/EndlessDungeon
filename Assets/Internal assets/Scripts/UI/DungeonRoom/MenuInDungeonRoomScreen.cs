using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.DungeonRoom
{
    public class MenuInDungeonRoomScreen : MonoBehaviour
    {
        [SerializeField] private Button exitButton;

        public void SetUp(UnityAction exitInMainLocation)
        {
            exitButton.onClick.AddListener(exitInMainLocation);
        }
    }
}
