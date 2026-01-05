using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace VaalTempleBuilder
{
    public class UIVisualFeedbackService
    {
        private readonly UIAnimationService _animationService;
        private readonly Dictionary<Transform, ElementState> _elementStates;
        private readonly Dictionary<Transform, Tweener> _activeColorTweens;
        private readonly Dictionary<Transform, Tweener> _activeScaleTweens;

        public UIVisualFeedbackService(UIAnimationService animationService)
        {
            _animationService = animationService;
            _elementStates = new Dictionary<Transform, ElementState>();
            _activeColorTweens = new Dictionary<Transform, Tweener>();
            _activeScaleTweens = new Dictionary<Transform, Tweener>();
        }

        public void RegisterElement(Transform element, Image image)
        {
            if (_elementStates.ContainsKey(element)) return;

            _elementStates[element] = new ElementState
            {
                Image = image,
                OriginalColor = image != null ? image.color : Color.white,
                OriginalScale = element.localScale,
                IsHovered = false,
                IsSelected = false
            };
        }

        public void UnregisterElement(Transform element)
        {
            KillTweens(element);
            _elementStates.Remove(element);
        }

        public void SetHovered(Transform element, bool isHovered)
        {
            if (!_elementStates.ContainsKey(element)) return;

            ElementState state = _elementStates[element];
            state.IsHovered = isHovered;

            UpdateElementVisuals(element, state);
        }

        public void SetSelected(Transform element, bool isSelected)
        {
            if (!_elementStates.ContainsKey(element)) return;

            ElementState state = _elementStates[element];
            state.IsSelected = isSelected;

            UpdateElementVisuals(element, state);
        }

        private void UpdateElementVisuals(Transform element, ElementState state)
        {
            KillTweens(element);

            if (state.IsSelected)
            {
                ApplySelectionVisuals(element, state);
            }
            else if (state.IsHovered)
            {
                ApplyHoverVisuals(element, state);
            }
            else
            {
                ApplyDefaultVisuals(element, state);
            }
        }

        private void ApplySelectionVisuals(Transform element, ElementState state)
        {
            if (state.Image != null)
            {
                Color selectionColor = _animationService.GetSelectionColor();
                _activeColorTweens[element] = _animationService.AnimateColorChange(state.Image, selectionColor);
            }

            Vector3 selectionScale = _animationService.GetSelectionScale(state.OriginalScale);
            _activeScaleTweens[element] = _animationService.AnimateScale(element, selectionScale);
        }

        private void ApplyHoverVisuals(Transform element, ElementState state)
        {
            if (state.Image != null)
            {
                Color hoverColor = _animationService.GetHoverColor();
                _activeColorTweens[element] = _animationService.AnimateColorChange(state.Image, hoverColor);
            }

            Vector3 hoverScale = _animationService.GetHoverScale(state.OriginalScale);
            _activeScaleTweens[element] = _animationService.AnimateScale(element, hoverScale);
        }

        private void ApplyDefaultVisuals(Transform element, ElementState state)
        {
            if (state.Image != null)
            {
                _activeColorTweens[element] = _animationService.AnimateColorChange(state.Image, state.OriginalColor);
            }

            _activeScaleTweens[element] = _animationService.AnimateScale(element, state.OriginalScale);
        }

        private void KillTweens(Transform element)
        {
            if (_activeColorTweens.TryGetValue(element, out Tweener colorTween))
            {
                colorTween?.Kill();
                _activeColorTweens.Remove(element);
            }

            if (_activeScaleTweens.TryGetValue(element, out Tweener scaleTween))
            {
                scaleTween?.Kill();
                _activeScaleTweens.Remove(element);
            }
        }

        public void Dispose()
        {
            foreach (var tween in _activeColorTweens.Values)
            {
                tween?.Kill();
            }

            foreach (var tween in _activeScaleTweens.Values)
            {
                tween?.Kill();
            }

            _activeColorTweens.Clear();
            _activeScaleTweens.Clear();
            _elementStates.Clear();
        }

        private class ElementState
        {
            public Image Image;
            public Color OriginalColor;
            public Vector3 OriginalScale;
            public bool IsHovered;
            public bool IsSelected;
        }
    }
}