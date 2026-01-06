using System.Collections.Generic;
using Enums;

namespace VaalTempleBuilder
{
    public class TooltipModel
    {
        public RoomType RoomType { get; private set; }
        public string RoomName { get; private set; }
        public string Description { get; private set; }
        public List<RoomType> ConnectsTo { get; private set; }
        public List<RoomType> UpgradesOtherRooms { get; private set; }
        public List<RoomType> UpgradedByOtherRooms { get; private set; }
        public List<RoomType> EffectRooms { get; private set; }

        public TooltipModel(
            RoomType roomType,
            string roomName,
            string description,
            List<RoomType> connectsTo,
            List<RoomType> upgradesOtherRooms,
            List<RoomType> upgradedByOtherRooms,
            List<RoomType> effectRooms)
        {
            RoomType = roomType;
            RoomName = roomName;
            Description = description;
            ConnectsTo = connectsTo ?? new List<RoomType>();
            UpgradesOtherRooms = upgradesOtherRooms ?? new List<RoomType>();
            UpgradedByOtherRooms = upgradedByOtherRooms ?? new List<RoomType>();
            EffectRooms = effectRooms ?? new List<RoomType>();
        }
    }
}