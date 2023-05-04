using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private MainLocationSettings _mainLocationSettings;

        public override void InstallBindings()
        {
            BindSettings();
            BindUIFactory();
            BindAbstractFactory();
            BindAssetsAddressable();
            BindPersistentProgressService();
            BindSaveLoadInstancesWatcher();
            BindSaveLoadService();
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

        private void BindSettings()
        {
            Container.Bind<MainLocationSettings>().FromInstance(_mainLocationSettings).AsSingle();
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
    }
}