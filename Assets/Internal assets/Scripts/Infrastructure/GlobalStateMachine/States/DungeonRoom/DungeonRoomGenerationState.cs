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

        public async override void Enter()
        {
            var seed = Random.Range(0, 10000);
            var width = 35;
            var height = 35;
            var minRoomCount = 7;
            var maxRoomCount = 9;
            var minRoomSize = 3;
            var maxRoomSize = 5;

            var tileDungeon = new TileDungeon(
                seed,
                width,
                height,
                minRoomCount,
                maxRoomCount,
                minRoomSize,
                maxRoomSize);

            var dungeonMap = await tileDungeon.GetDungeon();
            PrintDungeonInConsole(dungeonMap);
            
            Context.StateMachine.SwitchState<DungeonRoomSetUpState, TileDungeon.Tile[,]>(dungeonMap);
        }

        private static void PrintDungeonInConsole(TileDungeon.Tile[,] dungeonMap)
        {
            var str = "";
            for (var y = 0; y < dungeonMap.GetLength(0); y++)
            {
                for (var x = 0; x < dungeonMap.GetLength(1); x++)
                {
                    switch (dungeonMap[y, x])
                    {
                        case TileDungeon.Tile.FLOOR:
                            str += " ";
                            break;
                        case TileDungeon.Tile.INTERIOR_WALL:
                            str += "#";
                            break;
                        case TileDungeon.Tile.EXTERIOR_WALL:
                            str += "*";
                            break;
                        case TileDungeon.Tile.ENTRY:
                            str += "$";
                            break;
                        case TileDungeon.Tile.EXIT:
                            str += "%";
                            break;
                    }
                }

                str += "\n";
            }

            Debug.Log(str);
        }
    }
}