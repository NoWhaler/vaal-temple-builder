using VaalTempleBuilder;
using Zenject;

namespace Installers
{
    public class TooltipInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TooltipService>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<TooltipPresenter>()
                .AsSingle()
                .NonLazy();
        }
    }
}
