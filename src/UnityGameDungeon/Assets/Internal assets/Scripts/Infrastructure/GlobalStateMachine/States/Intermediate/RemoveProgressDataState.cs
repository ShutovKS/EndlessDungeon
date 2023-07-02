#region

using System;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;

#endregion

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class RemoveProgressDataState : StateWithParam<GameInstance, (string sceneName, Type nextStateType)>
    {
        public RemoveProgressDataState(GameInstance context, ISaveLoadService saveLoadService) : base(context)
        {
            _saveLoadService = saveLoadService;
        }

        private readonly ISaveLoadService _saveLoadService;

        public override void Enter((string sceneName, Type nextStateType) getValue)
        {
            _saveLoadService.ClearProgress();
            Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                typeof(SceneLoadingState),
                (getValue.sceneName, getValue.nextStateType));
        }
    }
}
