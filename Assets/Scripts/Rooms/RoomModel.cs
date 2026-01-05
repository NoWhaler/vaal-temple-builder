using Enums;
using UnityEngine;

namespace VaalTempleBuilder
{
    public class RoomModel
    {
        public RoomType RoomType { get; private set; }
        public Sprite Icon { get; private set; }

        public RoomModel(RoomType roomType, Sprite icon)
        {
            RoomType = roomType;
            Icon = icon;
        }
    }
}