#region

using System;
using Data.Addressable;
using Data.Dynamic.Location;
using Infrastructure.GlobalStateMachine.StateMachine;
using Services.SaveLoad;
using UnityEngine;

#endregion

namespace Infrastructure.GlobalStateMachine.States.Intermediate
{
    public class LoadLastSavedLocationState : State<GameInstance>
    {
        public LoadLastSavedLocationState(GameInstance context, ISaveLoadService saveLoadService) : base(
            context)
        {
            _saveLoadService = saveLoadService;
        }

        private readonly ISaveLoadService _saveLoadService;

        public override void Enter()
        {
            var currentLocationType = _saveLoadService.LoadProgress().currentLocation.locationType;

            switch (currentLocationType)
            {
                case CurrentLocation.LocationType.Main:
                    Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                        typeof(SceneLoadingState),
                        (AssetsAddressableConstants.MAIN_LOCATION_SCENE_NAME, typeof(MainLocationSetUpState)));

                    break;
                case CurrentLocation.LocationType.DungeonRoom:
                    Context.StateMachine.SwitchState<(string sceneName, Type newStateType)>(
                        typeof(SceneLoadingState),
                        (AssetsAddressableConstants.DUNGEON_ROOM_SCENE_NAME, typeof(DungeonLocationGenerationState)));

                    break;
                case CurrentLocation.LocationType.DungeonBoss:
                    Debug.LogError("Boos ERROR");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
