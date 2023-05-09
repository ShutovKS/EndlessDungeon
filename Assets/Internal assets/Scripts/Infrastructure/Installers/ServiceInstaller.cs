using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.UIFactory;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private MainLocationSettings _mainLocationSettings;
        [SerializeField] private MainMenuSettings _mainMenuSettings;

        public override void InstallBindings()
        {
            BindAssetsAddressable();
            BindAbstractFactory();
            BindSettings();
            BindUIFactory();
            BindStaticDataService();
            BindSaveLoadInstancesWatcher();
            BindPersistentProgressService();
            BindSaveLoadService();
            BindEnemyFactory();
        }

        private void BindAssetsAddressable()
        {
            Container.BindInterfacesTo<AssetsAddressableService>().AsSingle();
        }

        private void BindUIFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindAbstractFactory()
        {
            Container.BindInterfacesTo<AbstractFactory>().AsSingle();
        }
        
        private void BindEnemyFactory()
        {
            Container.BindInterfacesTo<EnemyFactory>().AsSingle();
        }

        private void BindSettings()
        {
            Container.Bind<MainLocationSettings>().FromInstance(_mainLocationSettings).AsSingle();
            Container.Bind<MainMenuSettings>().FromInstance(_mainMenuSettings).AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
        }

        private void BindSaveLoadInstancesWatcher()
        {
            Container.BindInterfacesTo<SaveLoadInstancesWatcher>().AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
        }

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
        }
    }
}