using System;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class SceneLoadingState : StateTwoParam<GameInstance, string, Type>
    {
        public SceneLoadingState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public override async void Enter(string sceneName, Type newStateType)
        {
            await _uiFactory.CreateLoadingScreen();

            await Addressables.LoadSceneAsync(sceneName).Task;

            Context.StateMachine.SwitchState(newStateType);
        }
    }
}