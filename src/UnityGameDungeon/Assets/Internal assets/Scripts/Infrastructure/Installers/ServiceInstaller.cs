#region

using Data.LocationSettings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Services.AssetsAddressableService;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;
using Zenject;

#endregion

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private PlayerStaticDefaultData playerStaticDefaultData;
        [SerializeField] private MainLocationSettings mainLocationSettings;
        [SerializeField] private DungeonRoomSettings dungeonRoomSettings;
        [SerializeField] private MainMenuSettings mainMenuSettings;

        public override void InstallBindings()
        {
            BindPersistentProgressService();
            BindSaveLoadInstancesWatcher();
            BindAssetsAddressable();
            BindStaticDataService();
            BindSaveLoadService();
            BindSettings();
            BindFactory();
        }

        private void BindSettings()
        {
            Container.Bind<PlayerStaticDefaultData>().FromInstance(playerStaticDefaultData).AsSingle();
            Container.Bind<MainLocationSettings>().FromInstance(mainLocationSettings).AsSingle();
            Container.Bind<DungeonRoomSettings>().FromInstance(dungeonRoomSettings).AsSingle();
            Container.Bind<MainMenuSettings>().FromInstance(mainMenuSettings).AsSingle();
        }

        private void BindFactory()
        {
            Container.BindInterfacesTo<AbstractFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container.BindInterfacesTo<PersistentProgressService>().AsSingle();
        }

        private void BindSaveLoadInstancesWatcher()
        {
            Container.BindInterfacesTo<SaveLoadInstancesWatcher>().AsSingle();
        }

        private void BindAssetsAddressable()
        {
            Container.BindInterfacesTo<AssetsAddressableService>().AsSingle();
        }

        private void BindStaticDataService()
        {
            Container.BindInterfacesTo<StaticDataService>().AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container.BindInterfacesTo<SaveLoadService>().AsSingle();
        }
    }
}
