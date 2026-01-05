using System;
using System.Linq;
using Enums;
using Grid;
using UnityEngine;

namespace Services
{
    public class UniqueRoomService
    {
        private GridStateService _gridStateService;

        public event Action<int, int> OnRoomEraseRequested;

        public UniqueRoomService(GridStateService gridStateService)
        {
            _gridStateService = gridStateService;
        }

        public bool IsUniqueRoomType(RoomType roomType)
        {
            return GridConstants.UniqueRoomTypes.Contains(roomType);
        }

        public void HandleUniqueRoomPlacement(RoomType roomType)
        {
            if (!IsUniqueRoomType(roomType)) return;

            Vector2Int? existingPosition = _gridStateService.FindRoomPosition(roomType);

            if (existingPosition.HasValue)
            {
                Vector2Int position = existingPosition.Value;
                _gridStateService.RemoveRoom(position.x, position.y);
                OnRoomEraseRequested?.Invoke(position.x, position.y);
                Debug.Log($"Removed existing {roomType} from position ({position.x}, {position.y})");
            }
        }
    }
}