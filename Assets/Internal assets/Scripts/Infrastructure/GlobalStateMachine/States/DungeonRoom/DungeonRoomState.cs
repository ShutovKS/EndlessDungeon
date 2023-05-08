using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomState : State<GameInstance>
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private GameObject _locationScreen;

        public DungeonRoomState(GameInstance context, IUIFactory uiFactory, IAbstractFactory abstractFactory,
            IEnemyFactory enemyFactory) : base(context)
        {
            _uiFactory = uiFactory;
            _abstractFactory = abstractFactory;
            _enemyFactory = enemyFactory;
        }

        public override void Enter()
        {
            _uiFactory.DestroyMainMenuScreen();
            _uiFactory.DestroyLoadingScreen();
            _enemyFactory.AllDeadEnemies += Finish;
        }

        public override void Exit()
        {
            _enemyFactory.DestroyAllInstances();
            _abstractFactory.DestroyAllInstances();
        }

        private void Finish()
        {
            Context.StateMachine.SwitchState<DungeonRoomLoadingState>();
            Debug.Log("Finish");
        }
    }
}