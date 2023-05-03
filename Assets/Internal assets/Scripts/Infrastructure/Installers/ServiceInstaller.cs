using Data.Settings;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.UIFactory;
using Services.AssetsAddressableService;
using UnityEngine;
using UnityEngine.Serialization;
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
    }
}