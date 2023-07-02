#region

using Infrastructure.GlobalStateMachine.States;
using Zenject;

#endregion

namespace Infrastructure.Installers
{
    public class StateInstaller : MonoInstaller
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
