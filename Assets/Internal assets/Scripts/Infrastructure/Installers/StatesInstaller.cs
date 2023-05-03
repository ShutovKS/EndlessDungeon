using Infrastructure.GlobalStateMachine.States;
using Zenject;

namespace Infrastructure.Installers
{
    public class StatesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindBootstrapState();
        }

        private void BindBootstrapState()
        {
            Container.Bind<IInitializable>().To<BootstrapState>().AsSingle().NonLazy();
        }
    }
}
