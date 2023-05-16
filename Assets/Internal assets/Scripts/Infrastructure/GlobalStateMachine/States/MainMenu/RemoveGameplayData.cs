using System;
using Data.Addressable;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class RemoveGameplayData : State<GameInstance>
    {
        public RemoveGameplayData(GameInstance context, ISaveLoadService saveLoadService) : base(context)
        {
            _saveLoadService = saveLoadService;
        }

        private readonly ISaveLoadService _saveLoadService;

        public override void Enter()
        {
            _saveLoadService.ClearProgress();
            Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME,
                typeof(MainLocationSetUpState));
        }
    }
}