using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomState : State<GameInstance>
    {
        public DungeonRoomState(GameInstance context, IUIFactory uiFactory) :
            base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private GameObject _locationScreen;

        public override void Enter()
        {
            
            
            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
        }
    }
}