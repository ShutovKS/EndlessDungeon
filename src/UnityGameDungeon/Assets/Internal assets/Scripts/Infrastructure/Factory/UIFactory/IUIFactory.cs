#region

using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Infrastructure.Factory.UIFactory
{
    public interface IUIFactory : IUIInfo
    {
        public Task<GameObject> CreateLoadingScreen();
        public void DestroyLoadingScreen();

        public Task<GameObject> CreateMainLocationScreen();
        public void DestroyMainLocationScreen();

        public Task<GameObject> CreateMainMenuScreen();
        public void DestroyMainMenuScreen();

        public Task<GameObject> CreateMenuInDungeonRoomScreen();
        public void DestroyMenuInDungeonRoomScreen();

        public Task<GameObject> CreateMenuInMainLocationScreen();
        public void DestroyMenuInMainLocationScreen();

        public Task<GameObject> CreateSkillsBookScreen();
        public void DestroySkillsBookScreen();
    }
}
