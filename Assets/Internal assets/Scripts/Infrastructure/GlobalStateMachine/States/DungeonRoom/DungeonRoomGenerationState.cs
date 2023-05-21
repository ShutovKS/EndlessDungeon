using System.Collections.Generic;
using Data.Dynamic.Location;
using DungeonGenerator;
using DungeonGenerator.Tiles.Interface;
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
            }
            else
            {
                seed = progress.dungeonRoom.seed;
            }

            progress.currentLocation.locationType = CurrentLocation.LocationType.DungeonRoom;
            _persistentProgressService.SetProgress(progress);
            _saveLoadService.SaveProgress();

            var dungeonArchitecture = DungeonGenerator.DungeonGenerator.GetDungeon(seed);

            Context.StateMachine
                .SwitchState<DungeonRoomSetUpState, (ITile[,] dungeonMap, (int, int) playerPosition, List<(int, int)>
                    enemiesPosition)>(dungeonArchitecture);
        }
    }
}