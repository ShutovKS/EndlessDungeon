using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuScreen : MonoBehaviour
    {
        public event Action OnNewGameButtonClicked;
        public event Action OnLoadGameButtonClicked;
        public event Action OnExitButtonClicked;

        [SerializeField] private Button _newGameButtonClicked;
        [SerializeField] private Button _loadGameButtonClicked;
        [SerializeField] private Button _exitButtonClicked;

        private void Start()
        {
            _newGameButtonClicked.onClick.AddListener(NewGameButtonClicked);
            _loadGameButtonClicked.onClick.AddListener(LoadGameButtonClicked);
            _exitButtonClicked.onClick.AddListener(ExitButtonClicked);
        }

        private void NewGameButtonClicked()
        {
            OnNewGameButtonClicked?.Invoke();
        }

        private void LoadGameButtonClicked()
        {
            OnLoadGameButtonClicked?.Invoke();
        }

        private void ExitButtonClicked()
        {
            OnExitButtonClicked?.Invoke();
        }
    }
}