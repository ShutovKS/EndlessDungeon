using System;
using Data.Addressable;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Loot;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomState : State<GameInstance>
    {
        public DungeonRoomState(GameInstance context, IUIFactory uiFactory, ISaveLoadService saveLoadService,
            IAbstractFactory abstractFactory, IEnemyFactory enemyFactory,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher) : base(context)
        {
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _abstractFactory = abstractFactory;
            _enemyFactory = enemyFactory;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
        }

        private readonly IEnemyFactory _enemyFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAbstractFactory _abstractFactory;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        public override void Enter()
        {
            _uiFactory.DestroyLoadingScreen();
            _enemyFactory.AllDeadEnemies += Finish;
        }

        public override void Exit()
        {
            _enemyFactory.DestroyAllInstances();
            _abstractFactory.DestroyAllInstances();
            _saveLoadInstancesWatcher.ClearProgress();
        }

        private void Finish()
        {
            Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME,
                typeof(DungeonRoomGenerationState));

            _saveLoadService.SaveProgress();
        }
    }
}