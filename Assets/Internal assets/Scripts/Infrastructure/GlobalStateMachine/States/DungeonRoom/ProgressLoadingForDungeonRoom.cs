using Data.Dynamic;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;

namespace Infrastructure.GlobalStateMachine.States
{
    public class ProgressLoadingForDungeonRoom : State<GameInstance>
    {
        public ProgressLoadingForDungeonRoom(GameInstance context, ISaveLoadService saveLoadService,
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

        public override void Enter()
        {
            LoadProgressOrInitNew();

            InformProgressReaders();

            Context.StateMachine.SwitchState<DungeonRoomState>();
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