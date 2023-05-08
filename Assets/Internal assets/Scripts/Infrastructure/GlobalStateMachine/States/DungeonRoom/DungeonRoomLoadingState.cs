using Data.Addressable;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.StateMachine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.GlobalStateMachine.States
{
    public class DungeonRoomLoadingState : State<GameInstance>
    {
        public DungeonRoomLoadingState(GameInstance context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }

        private readonly IUIFactory _uiFactory;

        public override async void Enter()
        {
            await _uiFactory.CreateLoadingScreen();

            var asyncOperationHandle =
                await Addressables.LoadSceneAsync((AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME)).Task;

            Context.StateMachine.SwitchState<DungeonRoomGenerationState>();
        }
    }
}