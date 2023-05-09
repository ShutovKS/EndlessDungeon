using System.Threading.Tasks;
using GeneratorDungeons;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Units.Enemy;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpEnemyState : StateOneParam<GameInstance, TileDungeon>
    {
        public DungeonRoomSetUpEnemyState(GameInstance context, IEnemyFactory enemyFactory)
            : base(context)
        {
            _enemyFactory = enemyFactory;
        }

        private readonly IEnemyFactory _enemyFactory;

        private const float UNIT = 4.85f / 2;

        public override async void Enter(TileDungeon tileDungeon)
        {
            await SetUpEnemy(tileDungeon);

            Context.StateMachine.SwitchState<ProgressLoadingForDungeonRoom>();
        }

        private async Task SetUpEnemy(TileDungeon tileDungeon)
        {
            if (tileDungeon.EnemyPosition != null)
                foreach (var position in tileDungeon.EnemyPosition)
                {
                    var enemy = await _enemyFactory.CreateInstance(
                        EnemyType.Golem,
                        new Vector3(position.Item1 * UNIT, 2, position.Item2 * UNIT));

                    enemy.transform.rotation = new Quaternion(0, Random.Range(-180f, 180f), 0, 0);
                }
        }
    }
}