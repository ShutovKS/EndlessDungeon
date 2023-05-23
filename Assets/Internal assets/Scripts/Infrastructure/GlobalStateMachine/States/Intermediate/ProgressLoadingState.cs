#region

using System;
using Data.Dynamic;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;

#endregion

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class ProgressLoadingState : StateWithParam<GameInstance, Type>
    {
        public ProgressLoadingState(GameInstance context, ISaveLoadService saveLoadService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher,
            IPersistentProgressService persistentProgressService) : base(context)
        {
            _saveLoadService = saveLoadService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
            _persistentProgressService = persistentProgressService;
        }

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;
        private readonly ISaveLoadService _saveLoadService;

        public override void Enter(Type nextStateType)
        {
            LoadProgressOrInitNew();

            InformProgressReaders();

            Context.StateMachine.SwitchState(nextStateType);
        }

        private void LoadProgressOrInitNew()
        {
            _persistentProgressService.SetProgress(_saveLoadService.LoadProgress() ?? InitNewProgress());
        }

        private static Progress InitNewProgress()
        {
            return new Progress();
        }

        private void InformProgressReaders()
        {
            foreach (var progressLoadable in _saveLoadInstancesWatcher.ProgressLoadable)
            {
                progressLoadable.LoadProgress(_persistentProgressService.Progress);
            }
        }
    }
}
