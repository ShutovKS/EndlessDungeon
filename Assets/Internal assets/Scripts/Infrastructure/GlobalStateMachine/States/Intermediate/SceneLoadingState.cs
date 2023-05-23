#region

using System;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UnityEngine.AddressableAssets;

#endregion

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class SceneLoadingState : StateWithParam<GameInstance, (string sceneName, Type nextStateType)>
    {
        public SceneLoadingState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public override async void Enter((string sceneName, Type nextStateType) getValue)
        {
            await _uiFactory.CreateLoadingScreen();

            await Addressables.LoadSceneAsync(getValue.sceneName).Task;

            Context.StateMachine.SwitchState(getValue.nextStateType);
        }
    }
}
