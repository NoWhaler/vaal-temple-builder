using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using VaalTempleBuilder;
using Zenject;

namespace Rooms
{
    public class RoomsPresenter : IInitializable, IDisposable
    {
        private PrefabContainer _prefabContainer;
        private UIRootProvider _uiRootProvider;
        private RoomSelectionService _roomSelectionService;

        private RoomsModel _model;
        private RoomsView _view;

        private readonly Dictionary<RoomView, RoomClickHandler> _roomClickHandlers = new Dictionary<RoomView, RoomClickHandler>();

        [Inject]
        private void Inject(PrefabContainer prefabContainer, UIRootProvider uiRootProvider, RoomSelectionService roomSelectionService)
        {
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
            _roomSelectionService = roomSelectionService;
        }

        public void Initialize()
        {
            _model = new RoomsModel();

            SpawnView();
            SubscribeToViewEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromViewEvents();

            if (_view != null)
            {
                _view.Dispose();
                UnityEngine.Object.Destroy(_view.gameObject);
                _view = null;
            }
        }

        private void SpawnView()
        {
            GameObject viewObject = _prefabContainer.InstantiatePrefab(
                PrefabType.MainScene,
                RoomsConstants.RoomsPrefabName,
                _uiRootProvider.UIRoot
            );

            if (viewObject != null)
            {
                _view = viewObject.GetComponent<RoomsView>();
                if (_view != null)
                {
                    _view.Initialize();
                }
                else
                {
                    Debug.LogError($"Rooms prefab does not have RoomsView component!");
                }
            }
        }

        private void SubscribeToViewEvents()
        {
            if (_view == null) return;

            foreach (var room in _view.Rooms.Values)
            {
                RoomClickHandler handler = new RoomClickHandler(this, room);
                _roomClickHandlers[room] = handler;
                room.OnRoomClicked += handler.OnClick;
            }
        }

        private void UnsubscribeFromViewEvents()
        {
            if (_view == null) return;

            foreach (var kvp in _roomClickHandlers)
            {
                kvp.Key.OnRoomClicked -= kvp.Value.OnClick;
            }

            _roomClickHandlers.Clear();
        }

        private void HandleRoomClick(RoomView roomView)
        {
            RoomType clickedRoomType = roomView.RoomType;

            if (_roomSelectionService.SelectedRoomType.HasValue && _roomSelectionService.SelectedRoomType.Value == clickedRoomType)
            {
                _roomSelectionService.DeselectRoom();
            }
            else
            {
                _roomSelectionService.SelectRoom(clickedRoomType, roomView.RoomIcon);
            }
        }

        private class RoomClickHandler
        {
            private readonly RoomsPresenter _presenter;
            private readonly RoomView _roomView;

            public RoomClickHandler(RoomsPresenter presenter, RoomView roomView)
            {
                _presenter = presenter;
                _roomView = roomView;
            }

            public void OnClick()
            {
                _presenter.HandleRoomClick(_roomView);
            }
        }
    }
}