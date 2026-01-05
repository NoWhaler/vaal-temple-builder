using System;
using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace VaalTempleBuilder
{
    public class RoomView : MonoBehaviour
    {
        [SerializeField] private RoomType _roomType;
        [SerializeField] private Image _roomIcon;
        [SerializeField] private Button _roomButton;

        public RoomType RoomType => _roomType;
        public Sprite RoomIcon => _roomIcon.sprite;

        public event Action OnRoomClicked;

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
    }
}