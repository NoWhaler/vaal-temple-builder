using System;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using VaalTempleBuilder;

namespace Medallions
{
    public class MedallionView : MonoBehaviour
    {
        [SerializeField] private MedallionType _medallionType;
        [SerializeField] private Image _medallionIcon;
        [SerializeField] private Button _medallionButton;

        public MedallionType MedallionType => _medallionType;

        public event Action OnMedallionClicked;

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
    }
}