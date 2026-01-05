using System;
using Enums;
using UnityEngine;

namespace VaalTempleBuilder
{
    public class RoomSelectionService
    {
        public RoomType? SelectedRoomType { get; private set; }
        public Sprite SelectedRoomIcon { get; private set; }

        public event Action<RoomType?> OnRoomSelectionChanged;

        public void SelectRoom(RoomType roomType, Sprite icon)
        {
            SelectedRoomType = roomType;
            SelectedRoomIcon = icon;
            OnRoomSelectionChanged?.Invoke(roomType);
            Debug.Log($"Room selected in service: {roomType}");
        }

        public void DeselectRoom()
        {
            SelectedRoomType = null;
            SelectedRoomIcon = null;
            OnRoomSelectionChanged?.Invoke(null);
            Debug.Log("Room deselected in service");
        }

        public bool HasSelection()
        {
            return SelectedRoomType.HasValue;
        }
    }
}