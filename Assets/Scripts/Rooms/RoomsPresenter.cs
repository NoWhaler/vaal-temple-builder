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
        private UIVisualFeedbackService _visualFeedbackService;

        private RoomsModel _model;
        private RoomsView _view;

        private readonly Dictionary<RoomView, RoomClickHandler> _roomClickHandlers = new Dictionary<RoomView, RoomClickHandler>();
        private readonly Dictionary<RoomView, RoomHoverHandler> _roomHoverHandlers = new Dictionary<RoomView, RoomHoverHandler>();
        private RoomView _currentlySelectedRoomView;

        [Inject]
        private void Inject(
            PrefabContainer prefabContainer,
            UIRootProvider uiRootProvider,
            RoomSelectionService roomSelectionService,
            UIVisualFeedbackService visualFeedbackService)
        {
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
            _roomSelectionService = roomSelectionService;
            _visualFeedbackService = visualFeedbackService;
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
                _visualFeedbackService.RegisterElement(room.transform, room.FrameImage);

                RoomClickHandler clickHandler = new RoomClickHandler(this, room);
                _roomClickHandlers[room] = clickHandler;
                room.OnRoomClicked += clickHandler.OnClick;

                RoomHoverHandler hoverHandler = new RoomHoverHandler(this, room);
                _roomHoverHandlers[room] = hoverHandler;
                room.OnRoomHoverEnter += hoverHandler.OnHoverEnter;
                room.OnRoomHoverExit += hoverHandler.OnHoverExit;
            }
        }

        private void UnsubscribeFromViewEvents()
        {
            if (_view == null) return;

            foreach (var kvp in _roomClickHandlers)
            {
                kvp.Key.OnRoomClicked -= kvp.Value.OnClick;
            }

            foreach (var kvp in _roomHoverHandlers)
            {
                kvp.Key.OnRoomHoverEnter -= kvp.Value.OnHoverEnter;
                kvp.Key.OnRoomHoverExit -= kvp.Value.OnHoverExit;
                _visualFeedbackService.UnregisterElement(kvp.Key.transform);
            }

            _roomClickHandlers.Clear();
            _roomHoverHandlers.Clear();
        }

        private void HandleRoomClick(RoomView roomView)
        {
            RoomType clickedRoomType = roomView.RoomType;

            if (_roomSelectionService.SelectedRoomType.HasValue && _roomSelectionService.SelectedRoomType.Value == clickedRoomType)
            {
                if (_currentlySelectedRoomView != null)
                {
                    _visualFeedbackService.SetSelected(_currentlySelectedRoomView.transform, false);
                    _currentlySelectedRoomView = null;
                }

                _roomSelectionService.DeselectRoom();
            }
            else
            {
                if (_currentlySelectedRoomView != null)
                {
                    _visualFeedbackService.SetSelected(_currentlySelectedRoomView.transform, false);
                }

                _currentlySelectedRoomView = roomView;
                _visualFeedbackService.SetSelected(roomView.transform, true);

                _roomSelectionService.SelectRoom(clickedRoomType, roomView.RoomIcon);
            }
        }

        private void HandleRoomHoverEnter(RoomView roomView)
        {
            _visualFeedbackService.SetHovered(roomView.transform, true);
        }

        private void HandleRoomHoverExit(RoomView roomView)
        {
            _visualFeedbackService.SetHovered(roomView.transform, false);
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

        private class RoomHoverHandler
        {
            private readonly RoomsPresenter _presenter;
            private readonly RoomView _roomView;

            public RoomHoverHandler(RoomsPresenter presenter, RoomView roomView)
            {
                _presenter = presenter;
                _roomView = roomView;
            }

            public void OnHoverEnter()
            {
                _presenter.HandleRoomHoverEnter(_roomView);
            }

            public void OnHoverExit()
            {
                _presenter.HandleRoomHoverExit(_roomView);
            }
        }
    }
}