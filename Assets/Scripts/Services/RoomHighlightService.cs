using System;
using System.Collections.Generic;
using Enums;

namespace VaalTempleBuilder
{
    public class RoomHighlightService
    {
        public event Action<List<RoomType>> OnRoomsHighlightRequested;
        public event Action OnRoomsHighlightCleared;

        public void RequestHighlight(List<RoomType> roomTypes)
        {
            OnRoomsHighlightRequested?.Invoke(roomTypes);
        }

        public void ClearHighlight()
        {
            OnRoomsHighlightCleared?.Invoke();
        }
    }
}
