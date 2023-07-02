#region

using UnityEngine;

#endregion

namespace Infrastructure.Factory.UIFactory
{
    public interface IUIInfo
    {
        public GameObject LoadingScreen { get; }
        public GameObject MainLocationScreen { get; }
        public GameObject MainMenuScreen { get; }
        public GameObject MenuInDungeonRoomScreen { get; }
        public GameObject MenuInMainLocationScreen { get; }
        public GameObject SkillsBookScreen { get; }
    }
}
