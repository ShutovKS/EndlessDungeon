using Infrastructure.GlobalStateMachine.StateMachine;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States
{
    public class ProgressLoadingState : StateOneParam<GameInstance, GameObject>
    {
        public ProgressLoadingState(GameInstance context) : base(context)
        {
        }

        public override void Enter(GameObject player)
        {
            // загрузка прогреса
            // сообщить читателям о прогрессе

            Context.StateMachine.SwitchState<MainLocationGameplayState, GameObject>(player);
        }
    }
}