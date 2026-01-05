using System;
using Enums;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VaalTempleBuilder
{
    public class RoomView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RoomType _roomType;
        [SerializeField] private Image _roomIcon;
        [SerializeField] private Image _frameImage;
        [SerializeField] private Button _roomButton;

        public RoomType RoomType => _roomType;
        public Sprite RoomIcon => _roomIcon.sprite;
        public Image FrameImage => _frameImage;

        public event Action OnRoomClicked;
        public event Action OnRoomHoverEnter;
        public event Action OnRoomHoverExit;

        public void Initialize()
        {
            _roomButton.onClick.AddListener(HandleRoomClick);
        }

        public void Dispose()
        {
            _roomButton.onClick.RemoveAllListeners();
        }

        private void HandleRoomClick()
        {
            OnRoomClicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnRoomHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnRoomHoverExit?.Invoke();
        }
    }
}