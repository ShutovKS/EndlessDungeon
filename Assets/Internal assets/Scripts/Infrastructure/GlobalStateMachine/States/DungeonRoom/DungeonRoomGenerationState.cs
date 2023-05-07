using GeneratorDungeons;
using Infrastructure.GlobalStateMachine.StateMachine;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomGenerationState : State<GameInstance>
    {
        public DungeonRoomGenerationState(GameInstance context) : base(context)
        {
        }

        public override async void Enter()
        {
            var seed = Random.Range(0, 10000);
            var width = 45;
            var height = 45;
            var minRoomCount = 6;
            var maxRoomCount = 11;
            var minRoomSize = 4;
            var maxRoomSize = 8;

            var tileDungeon = new TileDungeon(
                seed,
                width,
                height,
                minRoomCount,
                maxRoomCount,
                minRoomSize,
                maxRoomSize);

            await tileDungeon.Generation();

            Context.StateMachine.SwitchState<DungeonRoomSetUpState, TileDungeon>(tileDungeon);
        }
    }
}