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

        private MedallionsModel _model;
        private MedallionsView _view;

        private MedallionType? _selectedMedallionType;
        private readonly Dictionary<MedallionView, MedallionClickHandler> _medallionClickHandlers = new Dictionary<MedallionView, MedallionClickHandler>();

        [Inject]
        private void Inject(PrefabContainer prefabContainer, UIRootProvider uiRootProvider)
        {
            _prefabContainer = prefabContainer;
            _uiRootProvider = uiRootProvider;
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
                MedallionClickHandler handler = new MedallionClickHandler(this, medallion);
                _medallionClickHandlers[medallion] = handler;
                medallion.OnMedallionClicked += handler.OnClick;
            }
        }

        private void UnsubscribeFromViewEvents()
        {
            if (_view == null) return;

            foreach (var kvp in _medallionClickHandlers)
            {
                kvp.Key.OnMedallionClicked -= kvp.Value.OnClick;
            }

            _medallionClickHandlers.Clear();
        }

        private void HandleMedallionClick(MedallionView medallionView)
        {
            MedallionType clickedMedallionType = medallionView.MedallionType;

            if (_selectedMedallionType.HasValue && _selectedMedallionType.Value == clickedMedallionType)
            {
                _selectedMedallionType = null;
                Debug.Log($"Medallion deselected: {clickedMedallionType}");
            }
            else
            {
                _selectedMedallionType = clickedMedallionType;
                Debug.Log($"Medallion selected: {clickedMedallionType}");
            }
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
    }
}