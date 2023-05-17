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
        public GameObject MainLocationScreen { get; private set; }

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
    }
}