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

        public override void Enter()
        {
            var mapDungeon = TileDungeon.GenerationDungeon(
                Random.Range(0, 10000),
                4,
                8,
                6,
                11,
                45,
                45);

            Context.StateMachine.SwitchState<DungeonRoomSetUpState, MapDungeon>(mapDungeon);
        }
    }
}