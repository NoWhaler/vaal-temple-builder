using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace VaalTempleBuilder
{
    public class TooltipPresenter : IInitializable, IDisposable
    {
        private TooltipService _tooltipService;
        private PrefabContainer _prefabContainer;
        private UIRootProvider _uiRootProvider;

        private TooltipView _view;

        [Inject]
        private void Inject(
            TooltipService tooltipService,
            PrefabContainer prefabContainer,
            UIRootProvider uiRootProvider)
        {
            _tooltipService = tooltipService;
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
        }

        public void Initialize()
        {
            SpawnView();
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.Dispose();
                Object.Destroy(_view.gameObject);
                _view = null;
            }
        }

        private void SpawnView()
        {
            _view = _prefabContainer.InstantiatePrefab<TooltipView>(
                PrefabType.MainScene,
                TooltipConstants.TooltipPrefabName,
                _uiRootProvider.UIRoot
            );

            if (_view != null)
            {
                _view.Initialize();
            }
            else
            {
                Debug.LogError($"Failed to instantiate Tooltip prefab '{TooltipConstants.TooltipPrefabName}'");
            }
        }

        public void ShowTooltip(RoomType roomType, Vector3 position)
        {
            if (_view == null) return;

            TooltipModel tooltipData = _tooltipService.GetTooltipData(roomType);

            PopulateTooltipContent(tooltipData);
            _view.SetPosition(CalculateTooltipPosition(position));
            _view.Show();
        }

        public void HideTooltip()
        {
            if (_view == null) return;

            _view.Hide();
            ClearAllContainers();
        }

        private void PopulateTooltipContent(TooltipModel tooltipData)
        {
            _view.SetRoomName(tooltipData.RoomName);
            _view.SetDescription(tooltipData.Description);

            ClearAllContainers();

            PopulateRoomContainer(_view.RoomConnectionsContainer, tooltipData.ConnectsTo);
            PopulateRoomContainer(_view.UpgradeOtherRoomsContainer, tooltipData.UpgradesOtherRooms);
            PopulateRoomContainer(_view.UpgradedByOtherRoomsContainer, tooltipData.UpgradedByOtherRooms);
            PopulateRoomContainer(_view.EffectRoomsContainer, tooltipData.EffectRooms);

            _view.SetContainerVisible(_view.RoomConnectionsContainer, tooltipData.ConnectsTo.Count > 0);
            _view.SetContainerVisible(_view.UpgradeOtherRoomsContainer, tooltipData.UpgradesOtherRooms.Count > 0);
            _view.SetContainerVisible(_view.UpgradedByOtherRoomsContainer, tooltipData.UpgradedByOtherRooms.Count > 0);
            _view.SetContainerVisible(_view.EffectRoomsContainer, tooltipData.EffectRooms.Count > 0);
        }

        private void PopulateRoomContainer(Transform container, List<RoomType> roomTypes)
        {
            if (container == null) return;

            foreach (RoomType roomType in roomTypes)
            {
                Sprite roomIcon = _tooltipService.GetRoomIcon(roomType);
                if (roomIcon != null)
                {
                    TooltipRoomView roomView = SpawnTooltipRoomView(container);
                    if (roomView != null)
                    {
                        roomView.SetRoomIcon(roomIcon);
                    }
                }
            }
        }

        private TooltipRoomView SpawnTooltipRoomView(Transform parent)
        {
            GameObject roomPrefab = _prefabContainer.InstantiatePrefab(
                PrefabType.MainScene,
                TooltipConstants.TooltipRoomPrefabName,
                parent
            );

            if (roomPrefab != null)
            {
                return roomPrefab.GetComponent<TooltipRoomView>();
            }

            return null;
        }

        private void ClearAllContainers()
        {
            _view.ClearContainer(_view.RoomConnectionsContainer);
            _view.ClearContainer(_view.UpgradeOtherRoomsContainer);
            _view.ClearContainer(_view.UpgradedByOtherRoomsContainer);
            _view.ClearContainer(_view.EffectRoomsContainer);
        }

        private Vector3 CalculateTooltipPosition(Vector3 hoveredElementPosition)
        {
            Vector3 tooltipPosition = hoveredElementPosition;
            tooltipPosition.x += TooltipConstants.TooltipOffsetX;
            tooltipPosition.y += TooltipConstants.TooltipOffsetY;
            return tooltipPosition;
        }
    }
}
