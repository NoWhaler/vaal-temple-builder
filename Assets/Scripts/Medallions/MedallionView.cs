using System;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VaalTempleBuilder;

namespace Medallions
{
    public class MedallionView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MedallionType _medallionType;
        [SerializeField] private Image _medallionIcon;
        [SerializeField] private Image _frameImage;
        [SerializeField] private Button _medallionButton;

        public MedallionType MedallionType => _medallionType;
        public Image FrameImage => _frameImage;

        public event Action OnMedallionClicked;
        public event Action OnMedallionHoverEnter;
        public event Action OnMedallionHoverExit;

        public void Initialize()
        {
            _medallionButton.onClick.AddListener(HandleMedallionClick);
        }

        public void SetIcon(Sprite icon)
        {
            _medallionIcon.sprite = icon;
        }

        public void Dispose()
        {
            _medallionButton.onClick.RemoveAllListeners();
        }

        private void HandleMedallionClick()
        {
            OnMedallionClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnMedallionHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnMedallionHoverExit?.Invoke();
        }
    }
}