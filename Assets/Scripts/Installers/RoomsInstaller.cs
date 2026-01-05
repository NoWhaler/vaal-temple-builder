using Rooms;
using Zenject;

namespace Installers
{
    public class RoomsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RoomsPresenter>().AsSingle().NonLazy();
        }
    }
}