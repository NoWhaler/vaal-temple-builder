using Grid;
using Zenject;

namespace VaalTempleBuilder
{
    public class GridInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridPresenter>().AsSingle().NonLazy();
        }
    }
}