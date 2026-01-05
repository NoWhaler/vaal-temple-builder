using System;
using System.Collections.Generic;
using Data;
using Enums;
using ScriptableObjects;

namespace Services
{
    public class RoomConnectionService
    {
        private RoomConnectionData _connectionData;

        public RoomConnectionService(RoomConnectionData connectionData)
        {
            _connectionData = connectionData;
        }

        public bool CanConnect(RoomType roomA, RoomType roomB)
        {
            if (roomA == RoomType.ArchitectRestricted || roomB == RoomType.ArchitectRestricted)
            {
                return true;
            }

            foreach (RoomConnectionRule rule in _connectionData.ConnectionRules)
            {
                if (rule.RoomType == roomA && rule.ConnectsTo.Contains(roomB))
                {
                    return true;
                }
            }

            return false;
        }

        public List<RoomType> GetValidConnections(RoomType roomType)
        {
            if (roomType == RoomType.ArchitectRestricted)
            {
                return new List<RoomType>(Enum.GetValues(typeof(RoomType)) as RoomType[]);
            }

            foreach (RoomConnectionRule rule in _connectionData.ConnectionRules)
            {
                if (rule.RoomType == roomType)
                {
                    return new List<RoomType>(rule.ConnectsTo);
                }
            }

            return new List<RoomType>();
        }

        public List<RoomCountRestriction> GetCountRestrictions(RoomType roomType)
        {
            foreach (RoomConnectionRule rule in _connectionData.ConnectionRules)
            {
                if (rule.RoomType == roomType)
                {
                    return rule.CountRestrictions;
                }
            }

            return new List<RoomCountRestriction>();
        }

        public List<RoomChainExclusion> GetChainExclusions(RoomType roomType)
        {
            foreach (RoomConnectionRule rule in _connectionData.ConnectionRules)
            {
                if (rule.RoomType == roomType)
                {
                    return rule.ChainExclusions;
                }
            }

            return new List<RoomChainExclusion>();
        }

        public bool HasChainExclusionViolation(RoomType roomType, List<RoomType> chainRooms)
        {
            List<RoomChainExclusion> exclusions = GetChainExclusions(roomType);

            foreach (RoomChainExclusion exclusion in exclusions)
            {
                if (chainRooms.Contains(exclusion.ExcludedRoomType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
