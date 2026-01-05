using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using Services;
using UnityEngine;
using VaalTempleBuilder;
using Zenject;

namespace Grid
{
    public class GridPresenter : IInitializable, IDisposable
    {
        private PrefabContainer _prefabContainer;
        private UIRootProvider _uiRootProvider;
        private RoomSelectionService _roomSelectionService;
        private GridStateService _gridStateService;
        private ConnectionValidationService _connectionValidationService;
        private UIVisualFeedbackService _visualFeedbackService;

        private GridModel _gridModel;
        private GridView _gridView;

        private readonly List<GridCellPresenter> _gridCellPresenters = new List<GridCellPresenter>();

        [Inject]
        private void Inject(
            PrefabContainer prefabContainer,
            UIRootProvider uiRootProvider,
            RoomSelectionService roomSelectionService,
            GridStateService gridStateService,
            ConnectionValidationService connectionValidationService,
            UIVisualFeedbackService visualFeedbackService)
        {
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
            _roomSelectionService = roomSelectionService;
            _gridStateService = gridStateService;
            _connectionValidationService = connectionValidationService;
            _visualFeedbackService = visualFeedbackService;
        }

        public void Initialize()
        {
            _gridModel = new GridModel(GridConstants.DefaultGridWidth, GridConstants.DefaultGridHeight);

            SpawnGridView();
            GenerateGrid();
            SubscribeToEvents();
            UpdateAllConnectionCounts();
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
            ClearGrid();

            if (_gridView != null)
            {
                UnityEngine.Object.Destroy(_gridView.gameObject);
                _gridView = null;
            }
        }

        private void SpawnGridView()
        {
            GameObject gridPrefab = _prefabContainer.InstantiatePrefab(
                PrefabType.MainScene,
                GridConstants.GridPrefabName,
                _uiRootProvider.UIRoot
            );

            if (gridPrefab != null)
            {
                _gridView = gridPrefab.GetComponent<GridView>();
                if (_gridView == null)
                {
                    Debug.LogError($"Grid prefab does not have GridView component!");
                }
            }
        }

        private void GenerateGrid()
        {
            if (_gridView == null)
            {
                Debug.LogError("GridView is not spawned!");
                return;
            }

            if (_gridView.CellContainer == null)
            {
                Debug.LogError("Grid CellContainer is not assigned!");
                return;
            }

            ClearGrid();

            for (int y = 0; y < _gridModel.Height; y++)
            {
                for (int x = 0; x < _gridModel.Width; x++)
                {
                    CreateGridCell(x, y);
                }
            }
        }

        private void CreateGridCell(int x, int y)
        {
            GridCellModel cellModel = new GridCellModel(x, y);

            GridCellPresenter cellPresenter = new GridCellPresenter(
                cellModel,
                _prefabContainer,
                _roomSelectionService,
                _gridStateService,
                _connectionValidationService,
                _visualFeedbackService,
                _gridView.CellContainer
            );

            cellPresenter.Initialize();
            _gridCellPresenters.Add(cellPresenter);
        }

        private void ClearGrid()
        {
            foreach (var cellPresenter in _gridCellPresenters)
            {
                cellPresenter?.Dispose();
            }

            _gridCellPresenters.Clear();
        }

        private void SubscribeToEvents()
        {
            _roomSelectionService.OnRoomSelectionChanged += HandleRoomSelectionChanged;
            _gridStateService.OnGridStateChanged += HandleGridStateChanged;
        }

        private void UnsubscribeFromEvents()
        {
            _roomSelectionService.OnRoomSelectionChanged -= HandleRoomSelectionChanged;
            _gridStateService.OnGridStateChanged -= HandleGridStateChanged;
        }

        private void HandleRoomSelectionChanged(RoomType? roomType)
        {
            UpdateAllConnectionCounts();
        }

        private void HandleGridStateChanged()
        {
            UpdateAllConnectionCounts();
        }

        private void UpdateAllConnectionCounts()
        {
            foreach (GridCellPresenter cellPresenter in _gridCellPresenters)
            {
                cellPresenter.UpdateConnectionCount();
            }
        }
    }
}