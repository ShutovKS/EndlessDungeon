#region

using Data.Dynamic.Location;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;

#endregion

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonLocationGenerationState : State<GameInstance>
    {
        public DungeonLocationGenerationState(
            GameInstance context,
            IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService) : base(context)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        public override async void Enter()
        {
            int seed;
            var progress = _saveLoadService.LoadProgress();

            if (progress.dungeonLocation.seed == 0)
            {
                seed = Random.Range(int.MinValue, int.MaxValue);
                progress.dungeonLocation.seed = seed;
            }
            else
            {
                seed = progress.dungeonLocation.seed;
            }

            progress.currentLocation.locationType = CurrentLocation.LocationType.DungeonRoom;
            _persistentProgressService.SetProgress(progress);
            _saveLoadService.SaveProgress();

            var dungeonArchitecture = await DungeonGenerator.DungeonGenerator.GetDungeon(seed);

            Context.StateMachine.SwitchState(typeof(DungeonLocationSetUpState), dungeonArchitecture);
        }
    }
}
