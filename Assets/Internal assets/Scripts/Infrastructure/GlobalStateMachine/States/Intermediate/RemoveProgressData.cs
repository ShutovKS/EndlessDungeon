using System;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class RemoveProgressData : StateWithParam<GameInstance, (string sceneName, Type nextStateType)>
    {
        public RemoveProgressData(GameInstance context, ISaveLoadService saveLoadService) : base(context)
        {
            _saveLoadService = saveLoadService;
        }

        private readonly ISaveLoadService _saveLoadService;

        public override void Enter((string sceneName, Type nextStateType) getValue)
        {
            _saveLoadService.ClearProgress();
            Context.StateMachine.SwitchState<SceneLoadingState, (string sceneName, Type newStateType)>(
                (getValue.sceneName, getValue.nextStateType));
        }
    }
}