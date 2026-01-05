using UnityEngine;
using VaalTempleBuilder;
using Zenject;

namespace Installers
{
    public class UIRootInstaller : MonoInstaller
    {
        [SerializeField] private UIRootProvider _uiRootProvider;

        public override void InstallBindings()
        {
            Container.BindInstance(_uiRootProvider).AsSingle();
        }
    }
}