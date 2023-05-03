using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UI.MainLocation;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationGameplayState: StateOneParam<GameInstance, GameObject>
    {
        public MainLocationGameplayState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;
        private GameObject _mainLocationScreen;

        public override async void Enter(GameObject player)
        {
            _mainLocationScreen = await _uiFactory.CreateMainLocationScreen();

            if (_mainLocationScreen.TryGetComponent<MainLocationScreen>(out var mainLocationScreen))
            {
                
            }
            
            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
        }
    }
}