using System.Collections.Generic;
using Data.Dynamic.Location;
using DungeonGenerator;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomGenerationState : State<GameInstance>
    {
        public DungeonRoomGenerationState(GameInstance context, IPersistentProgressService persistentProgressService,
            ISaveLoadService saveLoadService) :
            base(context)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadService = saveLoadService;
        }

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadService _saveLoadService;

        public override void Enter()
        {
            int seed;
            var progress = _saveLoadService.LoadProgress();

            if (progress.dungeonRoom.seed == 0)
            {
                seed = Random.Range(int.MinValue, int.MaxValue);
                progress.dungeonRoom.seed = seed;
                progress.currentLocation.locationType = CurrentLocation.LocationType.DungeonRoom;
                _persistentProgressService.SetProgress(progress);
                _saveLoadService.SaveProgress();
            }
            else
            {
                seed = progress.dungeonRoom.seed;
            }
            
            var dungeonMapAndEnemiesPosition = DungeonGenerator.DungeonGenerator.GetDungeon(seed);
            Context.StateMachine.SwitchState<DungeonRoomSetUpState, (DungeonTilesType[,], List<(int, int)>)>(
                (dungeonMapAndEnemiesPosition.dungeonMap, dungeonMapAndEnemiesPosition.enemyPosition));
        }
    }
}