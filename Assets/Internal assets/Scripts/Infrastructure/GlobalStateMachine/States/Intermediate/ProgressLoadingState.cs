using System;
using Data.Dynamic;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class ProgressLoadingState : StateOneParam<GameInstance, Type>
    {
        public ProgressLoadingState(GameInstance context, ISaveLoadService saveLoadService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService) : base(context)
        {
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _persistentProgressService = persistentProgressService;
        }

        private readonly ISaveLoadService _saveLoadService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly IPersistentProgressService _persistentProgressService;

        public override void Enter(Type dungeonMapAndEnemiesPosition)
        {
            LoadProgressOrInitNew();

            InformProgressReaders();

            Context.StateMachine.SwitchState(dungeonMapAndEnemiesPosition);
        }

        private void LoadProgressOrInitNew() =>
            _persistentProgressService.SetProgress(_saveLoadService.LoadProgress() ?? InitNewProgress());

        private Progress InitNewProgress() => new();

        private void InformProgressReaders()
        {
            foreach (var progressLoadable in _saveLoadInstancesWatcher.ProgressLoadable)
                progressLoadable.LoadProgress(_persistentProgressService.Progress);
        }
    }
}