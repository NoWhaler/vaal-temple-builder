using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UIVisualSettings", menuName = "Settings/UI Visual Settings")]
    public class UIVisualSettings : ScriptableObject
    {
        [Header("Colors")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _hoverColor = new Color(1f, 0.784f, 0.243f);
        [SerializeField] private Color _selectionColor = new Color(0.682f, 0.176f, 0.176f);

        [Header("Animation Settings")]
        [SerializeField] private float _colorTransitionDuration = 0.2f;
        [SerializeField] private float _scaleTransitionDuration = 0.15f;

        [Header("Scale Settings")]
        [SerializeField] private float _hoverScaleIncrease = 0.1f;
        [SerializeField] private float _selectionScaleIncrease = 0.08f;

        public Color DefaultColor => _defaultColor;
        public Color HoverColor => _hoverColor;
        public Color SelectionColor => _selectionColor;

        public float ColorTransitionDuration => _colorTransitionDuration;
        public float ScaleTransitionDuration => _scaleTransitionDuration;

        public float HoverScaleIncrease => _hoverScaleIncrease;
        public float SelectionScaleIncrease => _selectionScaleIncrease;
    }
}