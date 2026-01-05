using ScriptableObjects;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GridServicesInstaller : MonoInstaller
    {
        [SerializeField] private RoomConnectionData _roomConnectionData;

        public override void InstallBindings()
        {
            Container.BindInstance(_roomConnectionData).AsSingle();
            Container.Bind<GridStateService>().AsSingle().NonLazy();
            Container.Bind<RoomConnectionService>().AsSingle().NonLazy();
            Container.Bind<ConnectionValidationService>().AsSingle().NonLazy();
            Container.Bind<UniqueRoomService>().AsSingle().NonLazy();
        }
    }
}
