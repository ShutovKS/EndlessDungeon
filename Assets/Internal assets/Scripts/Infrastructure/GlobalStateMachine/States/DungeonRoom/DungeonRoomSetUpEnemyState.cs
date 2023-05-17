using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Units.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpEnemyState : StateOneParam<GameInstance, List<(int, int)>>
    {
        public DungeonRoomSetUpEnemyState(GameInstance context, IEnemyFactory enemyFactory)
            : base(context)
        {
            _enemyFactory = enemyFactory;
        }

        private readonly IEnemyFactory _enemyFactory;

        private const float UNIT = 4.85f / 2;

        public override async void Enter(List<(int, int)> dungeonMapAndEnemiesPosition)
        {
            await SetUpEnemy(dungeonMapAndEnemiesPosition);

            Context.StateMachine.SwitchState<ProgressLoadingState, Type>(typeof(DungeonRoomState));
        }

        private async Task SetUpEnemy(List<(int, int)> enemyPosition)
        {
            foreach (var position in enemyPosition)
            {
                var enemy = await _enemyFactory.CreateInstance(
                    EnemyType.Golem,
                    new Vector3(position.Item1 * UNIT, 2, position.Item2 * UNIT));

                enemy.transform.rotation = new Quaternion(0, Random.Range(-180f, 180f), 0, 0);
            }
        }
    }
}