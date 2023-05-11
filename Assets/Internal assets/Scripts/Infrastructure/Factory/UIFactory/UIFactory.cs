using System.Threading.Tasks;
using Data.Addressable;
using Services.AssetsAddressableService;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory.UIFactory
{
    public class UIFactory : IUIFactory
    {
        public UIFactory(DiContainer container, IAssetsAddressableService assetsAddressableService)
        {
            _container = container;
            _assetsAddressableService = assetsAddressableService;
        }

        private readonly IAssetsAddressableService _assetsAddressableService;

        private readonly DiContainer _container;

        public GameObject LoadingScreen { get; private set; }
        public GameObject MainMenuScreen { get; private set; }
        public GameObject MainLocationScreen { get; private set; }
        public GameObject MenuInMainLocationScreen { get; private set; }


        #region LoadingScreen

        public async Task<GameObject> CreateLoadingScreen()
        {
            var loadingScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(
                    AssetsAddressablesConstants
                        .LOADING_PROCESS_SCREEN);

            LoadingScreen = _container.InstantiatePrefab(loadingScreenPrefab);

            return LoadingScreen;
        }

        public void DestroyLoadingScreen()
        {
            if (LoadingScreen != null)
                Object.Destroy(LoadingScreen);
        }

        #endregion

        #region MainMenuScreen

        public async Task<GameObject> CreateMainMenuScreen()
        {
            var mainMenuScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_MENU_SCREEN);

            MainMenuScreen = _container.InstantiatePrefab(mainMenuScreenPrefab);

            return MainMenuScreen;
        }

        public void DestroyMainMenuScreen()
        {
            if (MainMenuScreen != null)
                Object.Destroy(MainMenuScreen);
        }

        #endregion

        #region MainLocationScreen

        public async Task<GameObject> CreateMainLocationScreen()
        {
            var gameplayScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_SCREEN);

            MainLocationScreen = _container.InstantiatePrefab(gameplayScreenPrefab);

            return MainLocationScreen;
        }

        public void DestroyMainLocationScreen()
        {
            if (MainLocationScreen != null)
                Object.Destroy(MainLocationScreen);
        }

        #endregion

        #region MenuInMainLocationScreen

        public async Task<GameObject> CreateMenuInMainLocationScreen()
        {
            var gameplayScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(
                    AssetsAddressablesConstants.MENU_IN_MAIN_LOCATION_SCREEN);

            MenuInMainLocationScreen = _container.InstantiatePrefab(gameplayScreenPrefab);

            return MenuInMainLocationScreen;
        }

        public void DestroyMenuInMainLocationScreen()
        {
            if (MenuInMainLocationScreen != null)
                Object.Destroy(MenuInMainLocationScreen);
        }

        #endregion
    }
}