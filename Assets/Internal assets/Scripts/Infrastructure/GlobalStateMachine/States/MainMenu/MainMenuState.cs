using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UI.MainMenu;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class MainMenuState : StateOneParam<GameInstance, GameObject>
    {
        public MainMenuState(GameInstance context, IUIFactory uiFactory, IAbstractFactory abstractFactory) : base(
            context)
        {
            _uiFactory = uiFactory;
            _abstractFactory = abstractFactory;
        }

        private readonly IUIFactory _uiFactory;
        private readonly IAbstractFactory _abstractFactory;
        private MainMenuScreen _mainMenuScreen;

        public override async void Enter(GameObject mainMenuScreenInstance)
        {
            _uiFactory.DestroyLoadingScreen();

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
            _abstractFactory.DestroyAllInstances();
        }

        private void ChangeStateToGameplay()
        {
            Context.StateMachine.SwitchState<MainLocationLoadingState>();
        }
    }
}