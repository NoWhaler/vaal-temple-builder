using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace VaalTempleBuilder
{
    public class UIAnimationService
    {
        private readonly UIVisualSettings _settings;

        public UIAnimationService(UIVisualSettings settings)
        {
            _settings = settings;
        }

        public Tweener AnimateColorChange(Image image, Color targetColor)
        {
            if (image == null) return null;

            return image.DOColor(targetColor, _settings.ColorTransitionDuration)
                .SetEase(Ease.OutQuad);
        }

        public Tweener AnimateScale(Transform transform, Vector3 targetScale)
        {
            if (transform == null) return null;

            return transform.DOScale(targetScale, _settings.ScaleTransitionDuration)
                .SetEase(Ease.OutQuad);
        }

        public Vector3 GetHoverScale(Vector3 originalScale)
        {
            float scaleMultiplier = 1f + _settings.HoverScaleIncrease;
            return originalScale * scaleMultiplier;
        }

        public Vector3 GetSelectionScale(Vector3 originalScale)
        {
            float scaleMultiplier = 1f + _settings.SelectionScaleIncrease;
            return originalScale * scaleMultiplier;
        }

        public Color GetHoverColor()
        {
            return _settings.HoverColor;
        }

        public Color GetSelectionColor()
        {
            return _settings.SelectionColor;
        }

        public Color GetDefaultColor()
        {
            return _settings.DefaultColor;
        }
    }
}