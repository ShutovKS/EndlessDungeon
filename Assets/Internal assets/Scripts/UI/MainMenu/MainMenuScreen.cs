using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;
        [Space]
        [SerializeField] private Button _confirmationTrueButton;
        [SerializeField] private Button _confirmationFalseButton;
        [Space]
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _confirmationPanel;

        private readonly UnityAction _exitGame = Application.Quit;
        private UnityAction _newGame;
        private UnityAction _loadGame;

        private bool _isInStockSave;

        public void SetUp(UnityAction newGame, UnityAction loadGame, bool isInStockSave)
        {
            _isInStockSave = isInStockSave;
            _newGame = newGame;
            _loadGame = loadGame;

            _loadGameButton.onClick.AddListener(LoadGame);
            _newGameButton.onClick.AddListener(NewGame);
            _exitButton.onClick.AddListener(ExitGame);

            _mainPanel.SetActive(true);
            _confirmationPanel.SetActive(false);

            _loadGameButton.gameObject.SetActive(_isInStockSave);
        }

        private void LoadGame()
        {
            _loadGame?.Invoke();
        }

        private void NewGame()
        {
            if (_isInStockSave) ActionConfirmation(_newGame);
            else _newGame?.Invoke();
        }

        private void ExitGame()
        {
            ActionConfirmation(_exitGame);
        }

        private void ActionConfirmation(UnityAction unityAction)
        {
            _mainPanel.SetActive(false);
            _confirmationPanel.SetActive(true);
            _confirmationTrueButton.onClick.AddListener(unityAction);
            _confirmationFalseButton.onClick.AddListener(
                () =>
                {
                    _confirmationFalseButton.onClick.RemoveAllListeners();
                    _confirmationTrueButton.onClick.RemoveAllListeners();
                    _mainPanel.SetActive(true);
                    _confirmationPanel.SetActive(false);
                });
        }
    }
}