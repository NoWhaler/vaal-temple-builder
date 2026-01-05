using ScriptableObjects;
using UnityEngine;
using VaalTempleBuilder;
using Zenject;

namespace Installers
{
    public class UIServicesInstaller : MonoInstaller
    {
        [SerializeField] private UIVisualSettings _uiVisualSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_uiVisualSettings).AsSingle();

            Container.Bind<UIAnimationService>()
                .AsSingle()
                .NonLazy();

            Container.Bind<UIVisualFeedbackService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
