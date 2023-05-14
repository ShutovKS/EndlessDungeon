using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _newGameButtonClicked;
        [SerializeField] private Button _loadGameButtonClicked;
        [SerializeField] private Button _exitButtonClicked;

        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _newGamePanel;
        [SerializeField] private GameObject _exitPanel;
        
        public void SetUp(UnityAction newGame, UnityAction loadGame)
        {
            _newGameButtonClicked.onClick.AddListener(newGame);
            _loadGameButtonClicked.onClick.AddListener(loadGame);
            _exitButtonClicked.onClick.AddListener(Application.Quit);   
        }

        public void ClearAction()
        {
            _newGameButtonClicked.onClick.RemoveAllListeners();
            _loadGameButtonClicked.onClick.RemoveAllListeners();
            _exitButtonClicked.onClick.RemoveAllListeners();
        }
    }
}