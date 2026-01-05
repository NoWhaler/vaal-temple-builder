using System;
using Enums;
using ScriptableObjects;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VaalTempleBuilder;
using Zenject;

namespace Grid
{
    public class GridCellPresenter : IInitializable, IDisposable
    {
        private GridCellModel _model;
        private GridCellView _view;

        private PrefabContainer _prefabContainer;
        private RoomSelectionService _roomSelectionService;
        private GridStateService _gridStateService;
        private ConnectionValidationService _connectionValidationService;
        private UIVisualFeedbackService _visualFeedbackService;
        private Transform _parent;

        public GridCellPresenter(
            GridCellModel model,
            PrefabContainer prefabContainer,
            RoomSelectionService roomSelectionService,
            GridStateService gridStateService,
            ConnectionValidationService connectionValidationService,
            UIVisualFeedbackService visualFeedbackService,
            Transform parent)
        {
            _model = model;
            _prefabContainer = prefabContainer;
            _roomSelectionService = roomSelectionService;
            _gridStateService = gridStateService;
            _connectionValidationService = connectionValidationService;
            _visualFeedbackService = visualFeedbackService;
            _parent = parent;
        }

        public void Initialize()
        {
            SpawnView();
            SubscribeToViewEvents();
            InitializeStartCell();
        }

        public void Dispose()
        {
            UnsubscribeFromViewEvents();

            if (_view != null)
            {
                UnityEngine.Object.Destroy(_view.gameObject);
                _view = null;
            }
        }

        private void SpawnView()
        {
            GameObject cellObject = _prefabContainer.InstantiatePrefab(
                PrefabType.MainScene,
                GridConstants.GridCellPrefabName,
                _parent
            );

            if (cellObject != null)
            {
                _view = cellObject.GetComponent<GridCellView>();
                if (_view != null)
                {
                    _view.Initialize();
                    _view.SetCellName(_model.X, _model.Y);
                }
                else
                {
                    Debug.LogError($"GridCell prefab does not have GridCellView component!");
                }
            }
        }

        private void InitializeStartCell()
        {
            if (!_model.IsStartCell) return;

            Sprite pathSprite = Resources.Load<Sprite>(GridConstants.PathSpritePath);
            if (pathSprite != null)
            {
                _view.PlaceRoom(pathSprite);
                _gridStateService.PlaceRoom(_model.X, _model.Y, RoomType.Path);
                Debug.Log($"Initialized start cell at ({_model.X}, {_model.Y}) with Path sprite");
            }
            else
            {
                Debug.LogError($"Failed to load Path sprite from {GridConstants.PathSpritePath}");
            }
        }

        private void SubscribeToViewEvents()
        {
            if (_view == null) return;

            _visualFeedbackService.RegisterElement(_view.transform, _view.FrameImage);

            _view.OnCellClicked += HandleCellClick;
            _view.OnCellHoverEnter += HandleCellHoverEnter;
            _view.OnCellHoverExit += HandleCellHoverExit;
        }

        private void UnsubscribeFromViewEvents()
        {
            if (_view == null) return;

            _view.OnCellClicked -= HandleCellClick;
            _view.OnCellHoverEnter -= HandleCellHoverEnter;
            _view.OnCellHoverExit -= HandleCellHoverExit;

            _visualFeedbackService.UnregisterElement(_view.transform);
        }

        private void HandleCellClick(PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
            {
                HandleLeftClick();
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                HandleRightClick();
            }
        }

        private void HandleLeftClick()
        {
            if (_model.IsStartCell)
            {
                Debug.Log($"Cell ({_model.X}, {_model.Y}) is the start cell and cannot be modified.");
                return;
            }

            if (_view.HasRoom)
            {
                Debug.Log($"Cell ({_model.X}, {_model.Y}) already has a room. Ignoring click.");
                return;
            }

            if (!_roomSelectionService.HasSelection())
            {
                Debug.Log($"No room selected. Cell ({_model.X}, {_model.Y}) clicked.");
                return;
            }

            RoomType selectedRoom = _roomSelectionService.SelectedRoomType.Value;

            if (!_connectionValidationService.CanPlaceRoom(_model.X, _model.Y, selectedRoom))
            {
                return;
            }

            UnityEngine.Sprite icon = _roomSelectionService.SelectedRoomIcon;

            _view.PlaceRoom(icon);
            _gridStateService.PlaceRoom(_model.X, _model.Y, selectedRoom);
            Debug.Log($"Placed {selectedRoom} at cell ({_model.X}, {_model.Y})");
        }

        private void HandleRightClick()
        {
            if (_model.IsStartCell)
            {
                Debug.Log($"Cell ({_model.X}, {_model.Y}) is the start cell and cannot be erased.");
                return;
            }

            if (!_view.HasRoom)
            {
                Debug.Log($"Cell ({_model.X}, {_model.Y}) is already empty. Ignoring right click.");
                return;
            }

            _view.EraseRoom();
            _gridStateService.RemoveRoom(_model.X, _model.Y);
            Debug.Log($"Erased room from cell ({_model.X}, {_model.Y})");
        }

        public void UpdateConnectionCount()
        {
            if (_model.IsStartCell || _view.HasRoom)
            {
                _view.HideConnectionCount();
                return;
            }

            if (!_gridStateService.HasAdjacentRoom(_model.X, _model.Y))
            {
                _view.HideConnectionCount();
                return;
            }

            int validRoomTypeCount = CountValidRoomTypes();
            _view.SetConnectionCount(validRoomTypeCount);
        }

        private int CountValidRoomTypes()
        {
            int count = 0;
            RoomType[] allRoomTypes = (RoomType[])System.Enum.GetValues(typeof(RoomType));

            foreach (RoomType roomType in allRoomTypes)
            {
                if (roomType == RoomType.ArchitectRestricted)
                {
                    continue;
                }

                if (_connectionValidationService.CanPlaceRoom(_model.X, _model.Y, roomType))
                {
                    count++;
                }
            }

            return count;
        }

        private void HandleCellHoverEnter()
        {
            _visualFeedbackService.SetHovered(_view.transform, true);
        }

        private void HandleCellHoverExit()
        {
            _visualFeedbackService.SetHovered(_view.transform, false);
        }
    }
}