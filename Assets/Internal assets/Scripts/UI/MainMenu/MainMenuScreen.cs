#region

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#endregion

namespace UI.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitButton;

        [Space] [SerializeField] private Button confirmationTrueButton;
        [SerializeField] private Button confirmationFalseButton;

        [Space] [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject confirmationPanel;

        private readonly UnityAction _exitGame = Application.Quit;

        private bool _isInStockSave;
        private UnityAction _loadGame;
        private UnityAction _newGame;

        public void SetUp(UnityAction newGame, UnityAction loadGame, bool isInStockSave)
        {
            _isInStockSave = isInStockSave;
            _newGame = newGame;
            _loadGame = loadGame;

            loadGameButton.onClick.AddListener(LoadGame);
            newGameButton.onClick.AddListener(NewGame);
            exitButton.onClick.AddListener(ExitGame);

            mainPanel.SetActive(true);
            confirmationPanel.SetActive(false);

            loadGameButton.gameObject.SetActive(_isInStockSave);
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
            mainPanel.SetActive(false);
            confirmationPanel.SetActive(true);
            confirmationTrueButton.onClick.AddListener(unityAction);
            confirmationFalseButton.onClick.AddListener(
                () =>
                {
                    confirmationFalseButton.onClick.RemoveAllListeners();
                    confirmationTrueButton.onClick.RemoveAllListeners();
                    mainPanel.SetActive(true);
                    confirmationPanel.SetActive(false);
                });
        }
    }
}
