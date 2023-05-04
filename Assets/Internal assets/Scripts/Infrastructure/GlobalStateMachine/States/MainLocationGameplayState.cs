using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using UI.MainLocation;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationGameplayState : StateOneParam<GameInstance, GameObject>
    {
        public MainLocationGameplayState(GameInstance context, IUIFactory uiFactory,
            IPersistentProgressService persistentProgressService, ISaveLoadService saveLoadService) : base(context)
        {
            _uiFactory = uiFactory;
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        private readonly IUIFactory _uiFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;
        private GameObject _mainLocationScreen;

        public override async void Enter(GameObject player)
        {
            _mainLocationScreen = await _uiFactory.CreateMainLocationScreen();

            if (_mainLocationScreen.TryGetComponent<MainLocationScreen>(out var mainLocationScreen))
            {
                mainLocationScreen.SetUp(_saveLoadService);
            }

            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
        }
    }
}