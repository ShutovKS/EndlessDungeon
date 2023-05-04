using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UI.MainMenu;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainMenuState : State<GameInstance>
    {
        public MainMenuState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;
        private MainMenuScreen _mainMenuScreen;

        public override async void Enter()
        {
            _uiFactory.DestroyLoadingScreen();

            var mainMenuScreenInstance = await _uiFactory.CreateMainMenuScreen();
            if (mainMenuScreenInstance.TryGetComponent<MainMenuScreen>(out var mainMenuScreen))
            {
                _mainMenuScreen = mainMenuScreen;

                _mainMenuScreen.OnNewGameButtonClicked += ChangeStateToGameplay;
                _mainMenuScreen.OnLoadGameButtonClicked += ChangeStateToGameplay;
                _mainMenuScreen.OnExitButtonClicked += Application.Quit;
            }
        }

        public override void Exit()
        {
            if (_mainMenuScreen != null)
            {
                _mainMenuScreen.OnNewGameButtonClicked -= ChangeStateToGameplay;
                _mainMenuScreen.OnLoadGameButtonClicked -= ChangeStateToGameplay;
                _mainMenuScreen.OnExitButtonClicked -= Application.Quit;
            }
            
            _uiFactory.DestroyMainMenuScreen();
        }

        private void ChangeStateToGameplay()
        {
            Context.StateMachine.SwitchState<MainLocationLoadingState>();
        }
    }
}