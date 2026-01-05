using VaalTempleBuilder;
using Zenject;

namespace Installers
{
    public class RoomSelectionServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RoomSelectionService>().AsSingle().NonLazy();
        }
    }
}