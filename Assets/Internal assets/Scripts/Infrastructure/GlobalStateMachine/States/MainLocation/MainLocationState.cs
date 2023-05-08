using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using UI.MainLocation;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

namespace Infrastructure.GlobalStateMachine.States
{
    public class MainLocationState : StateOneParam<GameInstance, GameObject>
    {
        public MainLocationState(GameInstance context, IUIFactory uiFactory, ISaveLoadService saveLoadService,
            IAbstractFactory abstractFactory) :
            base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _abstractFactory = abstractFactory;
        }

        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private GameObject _locationScreen;

        public override async void Enter(GameObject portal)
        {
            _locationScreen = await _uiFactory.CreateMainLocationScreen();

            if (_locationScreen.TryGetComponent<MainLocationScreen>(out var mainLocationScreen))
            {
                mainLocationScreen.SetUp(_saveLoadService);
            }

            if (portal.transform.GetChild(0).TryGetComponent<XRGrabInteractable>(out var xrGrabInteractable))
            {
                xrGrabInteractable.selectEntered.AddListener((SelectEnterEventArgs arg0) => StartDungeonConquest());
            }

            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
        }

        public override void Exit()
        {
            _abstractFactory.DestroyAllInstances();
            _uiFactory.DestroyMainLocationScreen();
        }

        private void StartDungeonConquest()
        {
            Context.StateMachine.SwitchState<DungeonRoomLoadingState>();
        }
    }
}