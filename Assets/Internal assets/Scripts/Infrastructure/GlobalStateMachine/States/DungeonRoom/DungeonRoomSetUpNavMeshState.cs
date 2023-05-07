using GeneratorDungeons;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpNavMeshState : StateOneParam<GameInstance, TileDungeon>
    {
        public DungeonRoomSetUpNavMeshState(GameInstance context, IAbstractFactory abstractFactory) : base(context)
        {
            _abstractFactory = abstractFactory;
        }

        private readonly IAbstractFactory _abstractFactory;

        const float UNIT = 4.85f / 2;

        public override async void Enter(TileDungeon tileDungeon)
        {
            var navMeshSurface = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero)
                .AddComponent<NavMeshSurface>();

            navMeshSurface.BuildNavMesh();
            
            Context.StateMachine.SwitchState<DungeonRoomSetUpEnemyState, TileDungeon>(tileDungeon);
        }
    }
}