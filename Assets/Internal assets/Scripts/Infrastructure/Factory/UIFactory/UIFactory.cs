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
        public GameObject GameplayScreen { get; private set; }

        public async Task<GameObject> CreateLoadingScreen()
        {
            var loadingScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.LOADING_PROCESS_SCREEN);

            LoadingScreen = _container.InstantiatePrefab(loadingScreenPrefab);

            return LoadingScreen;
        }

        public void DestroyLoadingScreen()
        {
            Object.Destroy(LoadingScreen);
        }

        public async Task<GameObject> CreateMainMenuScreen()
        {
            var mainMenuScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_MENU_SCREEN);

            MainMenuScreen = _container.InstantiatePrefab(mainMenuScreenPrefab);

            return MainMenuScreen;
        }

        public void DestroyMainMenuScreen()
        {
            Object.Destroy(MainMenuScreen);
        }

        public async Task<GameObject> CreateMainLocationScreen()
        {
            var gameplayScreenPrefab =
                await _assetsAddressableService.GetAsset<GameObject>(AssetsAddressablesConstants.MAIN_LOCATION_SCREEN);

            GameplayScreen = _container.InstantiatePrefab(gameplayScreenPrefab);

            return GameplayScreen;
        }

        public void DestroyMainLocationScreen()
        {
            Object.Destroy(GameplayScreen);
        }
    }
}

