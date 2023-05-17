using System.Collections.Generic;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using Unity.AI.Navigation;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomSetUpNavMeshState : StateOneParam<GameInstance, List<(int, int)>>
    {
        public DungeonRoomSetUpNavMeshState(GameInstance context, IAbstractFactory abstractFactory) : base(context)
        {
            _abstractFactory = abstractFactory;
        }

        private readonly IAbstractFactory _abstractFactory;

        public override void Enter(List<(int, int)> enemiesPosition)
        {
            var navMeshSurface = _abstractFactory.CreateInstance(new GameObject(), Vector3.zero)
                .AddComponent<NavMeshSurface>();

            navMeshSurface.BuildNavMesh();
            
            Context.StateMachine.SwitchState<DungeonRoomSetUpEnemyState, List<(int, int)>>(enemiesPosition);
        }
    }
}