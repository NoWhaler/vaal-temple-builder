using Medallions;
using Zenject;

namespace Installers
{
    public class MedallionsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MedallionsPresenter>().AsSingle().NonLazy();
        }
    }
}