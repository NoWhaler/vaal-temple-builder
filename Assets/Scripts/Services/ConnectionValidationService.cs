using System.Collections.Generic;
using Data;
using Enums;
using UnityEngine;

namespace Services
{
    public class ConnectionValidationService
    {
        private GridStateService _gridStateService;
        private RoomConnectionService _roomConnectionService;

        public ConnectionValidationService(
            GridStateService gridStateService,
            RoomConnectionService roomConnectionService)
        {
            _gridStateService = gridStateService;
            _roomConnectionService = roomConnectionService;
        }

        public bool CanPlaceRoom(int x, int y, RoomType roomType)
        {
            if (roomType == RoomType.ArchitectRestricted)
            {
                return true;
            }

            if (roomType == RoomType.Architect)
            {
                if (!IsArchitectPositionValid(x, y))
                {
                    Debug.Log($"Cannot place {roomType} at cell ({x}, {y}). Position restriction: Architect can only be placed at X: 1-3, Y: 1-7.");
                    return false;
                }
                return true;
            }

            if (!_gridStateService.HasAdjacentRoom(x, y))
            {
                Debug.Log($"Cannot place {roomType} at cell ({x}, {y}). Adjacency requirement: Must have at least one adjacent room.");
                return false;
            }

            if (!HasValidAdjacentConnection(x, y, roomType))
            {
                Debug.Log($"Cannot place {roomType} at cell ({x}, {y}). Connection requirement: No valid connections to adjacent rooms.");
                return false;
            }

            if (HasCountRestrictionViolation(x, y, roomType))
            {
                Debug.Log($"Cannot place {roomType} at cell ({x}, {y}). Count restriction: Maximum adjacent room count exceeded.");
                return false;
            }

            if (HasChainExclusionViolation(x, y, roomType))
            {
                Debug.Log($"Cannot place {roomType} at cell ({x}, {y}). Chain exclusion: Conflicting room chain detected.");
                return false;
            }

            return true;
        }

        private bool IsArchitectPositionValid(int x, int y)
        {
            return x >= 1 && x <= 3 && y >= 1 && y <= 7;
        }

        private bool HasValidAdjacentConnection(int x, int y, RoomType roomType)
        {
            List<RoomType> adjacentRooms = _gridStateService.GetAdjacentRooms(x, y);

            foreach (RoomType adjacentRoom in adjacentRooms)
            {
                if (_roomConnectionService.CanConnect(roomType, adjacentRoom))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasCountRestrictionViolation(int x, int y, RoomType roomType)
        {
            List<RoomCountRestriction> countRestrictions = _roomConnectionService.GetCountRestrictions(roomType);

            foreach (RoomCountRestriction restriction in countRestrictions)
            {
                int adjacentCount = _gridStateService.CountAdjacentRoomsOfType(x, y, restriction.RestrictedRoomType);

                if (adjacentCount > restriction.MaxAdjacentCount)
                {
                    return true;
                }
            }

            List<Vector2Int> adjacentPositions = _gridStateService.GetAdjacentPositions(x, y);

            foreach (Vector2Int adjacentPosition in adjacentPositions)
            {
                RoomType? adjacentRoomType = _gridStateService.GetRoom(adjacentPosition.x, adjacentPosition.y);

                if (adjacentRoomType.HasValue)
                {
                    List<RoomCountRestriction> adjacentRestrictions = _roomConnectionService.GetCountRestrictions(adjacentRoomType.Value);

                    foreach (RoomCountRestriction restriction in adjacentRestrictions)
                    {
                        if (restriction.RestrictedRoomType == roomType)
                        {
                            int currentCount = _gridStateService.CountAdjacentRoomsOfType(adjacentPosition.x, adjacentPosition.y, roomType);

                            if (currentCount >= restriction.MaxAdjacentCount)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool HasChainExclusionViolation(int x, int y, RoomType roomType)
        {
            List<Vector2Int> adjacentPositions = _gridStateService.GetAdjacentPositions(x, y);
            HashSet<RoomType> chainRooms = new HashSet<RoomType>();

            foreach (Vector2Int adjacentPosition in adjacentPositions)
            {
                if (_gridStateService.HasRoom(adjacentPosition.x, adjacentPosition.y))
                {
                    List<RoomType> connectedChain = _gridStateService.GetConnectedChain(adjacentPosition.x, adjacentPosition.y);

                    foreach (RoomType chainRoom in connectedChain)
                    {
                        chainRooms.Add(chainRoom);
                    }
                }
            }

            List<RoomType> chainRoomsList = new List<RoomType>(chainRooms);
            return _roomConnectionService.HasChainExclusionViolation(roomType, chainRoomsList);
        }

        public int GetValidConnectionCount(int x, int y, RoomType roomType)
        {
            if (roomType == RoomType.ArchitectRestricted)
            {
                return 4;
            }

            int validCount = 0;
            List<Vector2Int> adjacentPositions = _gridStateService.GetAdjacentPositions(x, y);

            foreach (Vector2Int position in adjacentPositions)
            {
                RoomType? adjacentRoom = _gridStateService.GetRoom(position.x, position.y);
                if (adjacentRoom.HasValue)
                {
                    if (_roomConnectionService.CanConnect(roomType, adjacentRoom.Value))
                    {
                        validCount++;
                    }
                }
            }

            return validCount;
        }
    }
}
