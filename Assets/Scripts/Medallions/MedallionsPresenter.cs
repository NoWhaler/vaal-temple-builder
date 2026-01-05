using System;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;
using VaalTempleBuilder;
using Zenject;

namespace Medallions
{
    public class MedallionsPresenter : IInitializable, IDisposable
    {
        private PrefabContainer _prefabContainer;
        private UIRootProvider _uiRootProvider;
        private UIVisualFeedbackService _visualFeedbackService;

        private MedallionsModel _model;
        private MedallionsView _view;

        private MedallionType? _selectedMedallionType;
        private readonly Dictionary<MedallionView, MedallionClickHandler> _medallionClickHandlers = new Dictionary<MedallionView, MedallionClickHandler>();
        private readonly Dictionary<MedallionView, MedallionHoverHandler> _medallionHoverHandlers = new Dictionary<MedallionView, MedallionHoverHandler>();
        private MedallionView _currentlySelectedMedallionView;

        [Inject]
        private void Inject(
            PrefabContainer prefabContainer,
            UIRootProvider uiRootProvider,
            UIVisualFeedbackService visualFeedbackService)
        {
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
            _visualFeedbackService = visualFeedbackService;
        }

        public void Initialize()
        {
            _model = new MedallionsModel();

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
                MedallionsConstants.MedallionsPrefabName,
                _uiRootProvider.UIRoot
            );

            if (viewObject != null)
            {
                _view = viewObject.GetComponent<MedallionsView>();
                if (_view != null)
                {
                    _view.Initialize();
                }
                else
                {
                    Debug.LogError($"Medallions prefab does not have MedallionsView component!");
                }
            }
        }

        private void SubscribeToViewEvents()
        {
            if (_view == null) return;

            foreach (var medallion in _view.Medallions.Values)
            {
                _visualFeedbackService.RegisterElement(medallion.transform, medallion.FrameImage);

                MedallionClickHandler clickHandler = new MedallionClickHandler(this, medallion);
                _medallionClickHandlers[medallion] = clickHandler;
                medallion.OnMedallionClicked += clickHandler.OnClick;

                MedallionHoverHandler hoverHandler = new MedallionHoverHandler(this, medallion);
                _medallionHoverHandlers[medallion] = hoverHandler;
                medallion.OnMedallionHoverEnter += hoverHandler.OnHoverEnter;
                medallion.OnMedallionHoverExit += hoverHandler.OnHoverExit;
            }
        }

        private void UnsubscribeFromViewEvents()
        {
            if (_view == null) return;

            foreach (var kvp in _medallionClickHandlers)
            {
                kvp.Key.OnMedallionClicked -= kvp.Value.OnClick;
            }

            foreach (var kvp in _medallionHoverHandlers)
            {
                kvp.Key.OnMedallionHoverEnter -= kvp.Value.OnHoverEnter;
                kvp.Key.OnMedallionHoverExit -= kvp.Value.OnHoverExit;
                _visualFeedbackService.UnregisterElement(kvp.Key.transform);
            }

            _medallionClickHandlers.Clear();
            _medallionHoverHandlers.Clear();
        }

        private void HandleMedallionClick(MedallionView medallionView)
        {
            MedallionType clickedMedallionType = medallionView.MedallionType;

            if (_selectedMedallionType.HasValue && _selectedMedallionType.Value == clickedMedallionType)
            {
                if (_currentlySelectedMedallionView != null)
                {
                    _visualFeedbackService.SetSelected(_currentlySelectedMedallionView.transform, false);
                    _currentlySelectedMedallionView = null;
                }

                _selectedMedallionType = null;
                Debug.Log($"Medallion deselected: {clickedMedallionType}");
            }
            else
            {
                if (_currentlySelectedMedallionView != null)
                {
                    _visualFeedbackService.SetSelected(_currentlySelectedMedallionView.transform, false);
                }

                _currentlySelectedMedallionView = medallionView;
                _visualFeedbackService.SetSelected(medallionView.transform, true);

                _selectedMedallionType = clickedMedallionType;
                Debug.Log($"Medallion selected: {clickedMedallionType}");
            }
        }

        private void HandleMedallionHoverEnter(MedallionView medallionView)
        {
            _visualFeedbackService.SetHovered(medallionView.transform, true);
        }

        private void HandleMedallionHoverExit(MedallionView medallionView)
        {
            _visualFeedbackService.SetHovered(medallionView.transform, false);
        }

        private class MedallionClickHandler
        {
            private readonly MedallionsPresenter _presenter;
            private readonly MedallionView _medallionView;

            public MedallionClickHandler(MedallionsPresenter presenter, MedallionView medallionView)
            {
                _presenter = presenter;
                _medallionView = medallionView;
            }

            public void OnClick()
            {
                _presenter.HandleMedallionClick(_medallionView);
            }
        }

        private class MedallionHoverHandler
        {
            private readonly MedallionsPresenter _presenter;
            private readonly MedallionView _medallionView;

            public MedallionHoverHandler(MedallionsPresenter presenter, MedallionView medallionView)
            {
                _presenter = presenter;
                _medallionView = medallionView;
            }

            public void OnHoverEnter()
            {
                _presenter.HandleMedallionHoverEnter(_medallionView);
            }

            public void OnHoverExit()
            {
                _presenter.HandleMedallionHoverExit(_medallionView);
            }
        }
    }
}