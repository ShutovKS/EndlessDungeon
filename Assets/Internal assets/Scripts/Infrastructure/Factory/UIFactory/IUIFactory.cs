using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Factory.UIFactory
{
    public interface IUIFactory : IUIInfo
    {
        public Task<GameObject> CreateLoadingScreen();
        public void DestroyLoadingScreen();
        public Task<GameObject> CreateMainMenuScreen();
        public void DestroyMainMenuScreen();
        public Task<GameObject> CreateMainLocationScreen();
        public void DestroyMainLocationScreen();
    }
}

