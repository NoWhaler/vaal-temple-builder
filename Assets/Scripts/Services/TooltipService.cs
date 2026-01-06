using System.Collections.Generic;
using Data;
using Enums;
using ScriptableObjects;
using UnityEngine;

namespace VaalTempleBuilder
{
    public class TooltipService
    {
        private readonly RoomConnectionData _roomConnectionData;
        private readonly Dictionary<RoomType, TooltipModel> _tooltipCache;
        private readonly Dictionary<RoomType, Sprite> _roomIconCache;

        public TooltipService(RoomConnectionData roomConnectionData)
        {
            _roomConnectionData = roomConnectionData;
            _tooltipCache = new Dictionary<RoomType, TooltipModel>();
            _roomIconCache = new Dictionary<RoomType, Sprite>();
            BuildCaches();
        }

        private void BuildCaches()
        {
            foreach (RoomConnectionRule rule in _roomConnectionData.ConnectionRules)
            {
                TooltipModel tooltipModel = new TooltipModel(
                    rule.RoomType,
                    rule.RoomName,
                    rule.Description,
                    rule.ConnectsTo,
                    rule.UpgradesOtherRooms,
                    rule.UpgradedByOtherRooms,
                    rule.EffectRooms
                );

                _tooltipCache[rule.RoomType] = tooltipModel;
                _roomIconCache[rule.RoomType] = rule.RoomIcon;
            }
        }

        public TooltipModel GetTooltipData(RoomType roomType)
        {
            if (_tooltipCache.TryGetValue(roomType, out TooltipModel tooltipModel))
            {
                return tooltipModel;
            }

            return new TooltipModel(
                roomType,
                "Unknown Room",
                "No description available.",
                new List<RoomType>(),
                new List<RoomType>(),
                new List<RoomType>(),
                new List<RoomType>()
            );
        }

        public Sprite GetRoomIcon(RoomType roomType)
        {
            if (_roomIconCache.TryGetValue(roomType, out Sprite icon))
            {
                return icon;
            }

            return null;
        }

        public bool HasTooltipData(RoomType roomType)
        {
            return _tooltipCache.ContainsKey(roomType);
        }
    }
}
