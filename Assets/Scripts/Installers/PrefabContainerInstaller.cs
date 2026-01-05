using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class PrefabContainerInstaller : MonoInstaller
    {
        [SerializeField] private PrefabContainer _prefabContainer;

        public override void InstallBindings()
        {
            _prefabContainer.Initialize(Container);
            Container.BindInstance(_prefabContainer).AsSingle();
        }
    }
}