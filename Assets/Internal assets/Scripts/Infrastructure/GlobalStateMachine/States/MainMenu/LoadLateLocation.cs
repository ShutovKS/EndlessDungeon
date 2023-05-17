using System;
using Data.Addressable;
using Data.Dynamic.Location;
using Infrastructure.GlobalStateMachine.StateMachine;
using Infrastructure.GlobalStateMachine.States.Intermediate;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.GlobalStateMachine.States.MainMenu
{
    public class LoadLateLocation : State<GameInstance>
    {
        public LoadLateLocation(GameInstance context, ISaveLoadService saveLoadService) : base(
            context)
        {
            _saveLoadService = saveLoadService;
        }

        private readonly ISaveLoadService _saveLoadService;

        public override void Enter()
        {
            Debug.Log(_saveLoadService.LoadProgress().currentLocation.locationType.ToString());
            switch (_saveLoadService.LoadProgress().currentLocation.locationType)
            {
                case CurrentLocation.LocationType.Main:
                    Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                        AssetsAddressablesConstants.MAIN_LOCATION_SCENE_NAME,
                        typeof(MainLocationSetUpState));

                    break;
                case CurrentLocation.LocationType.DungeonRoom:
                    Context.StateMachine.SwitchState<SceneLoadingState, string, Type>(
                        AssetsAddressablesConstants.DUNGEON_ROOM_SCENE_NAME,
                        typeof(DungeonRoomGenerationState));

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