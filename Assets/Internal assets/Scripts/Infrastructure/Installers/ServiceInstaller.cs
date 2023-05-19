using Data.Settings;
using Data.Static;
using Infrastructure.Factory.AbstractFactory;
using Infrastructure.Factory.EnemyFactory;
using Infrastructure.Factory.PlayerFactory;
using Infrastructure.Factory.UIFactory;
using Infrastructure.GlobalStateMachine.States;
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
        [SerializeField] private PlayerStaticDefaultData _playerStaticDefaultData;
        [SerializeField] private MainLocationSettings _mainLocationSettings;
        [SerializeField] private DungeonRoomSettings _dungeonRoomSettings;
        [SerializeField] private MainMenuSettings _mainMenuSettings;

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

        private void BindAssetsAddressable()
        {
            Container.BindInterfacesTo<AssetsAddressableService>().AsSingle();
        }

        private void BindFactory()
        {
            Container.BindInterfacesTo<AbstractFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
        }

        private void BindSettings()
        {
            Container.Bind<PlayerStaticDefaultData>().FromInstance(_playerStaticDefaultData).AsSingle();
            Container.Bind<MainLocationSettings>().FromInstance(_mainLocationSettings).AsSingle();
            Container.Bind<DungeonRoomSettings>().FromInstance(_dungeonRoomSettings).AsSingle();
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