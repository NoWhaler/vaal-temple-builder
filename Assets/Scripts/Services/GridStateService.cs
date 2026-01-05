using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Services
{
    public class GridStateService
    {
        private Dictionary<Vector2Int, RoomType> _placedRooms = new Dictionary<Vector2Int, RoomType>();

        public event Action OnGridStateChanged;

        public void PlaceRoom(int x, int y, RoomType roomType)
        {
            Vector2Int position = new Vector2Int(x, y);
            _placedRooms[position] = roomType;
            OnGridStateChanged?.Invoke();
        }

        public void RemoveRoom(int x, int y)
        {
            Vector2Int position = new Vector2Int(x, y);
            _placedRooms.Remove(position);
            OnGridStateChanged?.Invoke();
        }

        public bool HasRoom(int x, int y)
        {
            Vector2Int position = new Vector2Int(x, y);
            return _placedRooms.ContainsKey(position);
        }

        public RoomType? GetRoom(int x, int y)
        {
            Vector2Int position = new Vector2Int(x, y);
            if (_placedRooms.TryGetValue(position, out RoomType roomType))
            {
                return roomType;
            }
            return null;
        }

        public List<Vector2Int> GetAdjacentPositions(int x, int y)
        {
            return new List<Vector2Int>
            {
                new Vector2Int(x - 1, y),
                new Vector2Int(x + 1, y),
                new Vector2Int(x, y - 1),
                new Vector2Int(x, y + 1)
            };
        }

        public List<RoomType> GetAdjacentRooms(int x, int y)
        {
            List<RoomType> adjacentRooms = new List<RoomType>();
            List<Vector2Int> adjacentPositions = GetAdjacentPositions(x, y);

            foreach (Vector2Int position in adjacentPositions)
            {
                if (_placedRooms.TryGetValue(position, out RoomType roomType))
                {
                    adjacentRooms.Add(roomType);
                }
            }

            return adjacentRooms;
        }

        public bool HasAdjacentRoom(int x, int y)
        {
            List<Vector2Int> adjacentPositions = GetAdjacentPositions(x, y);

            foreach (Vector2Int position in adjacentPositions)
            {
                if (_placedRooms.ContainsKey(position))
                {
                    return true;
                }
            }

            return false;
        }

        public int CountAdjacentRoomsOfType(int x, int y, RoomType roomType)
        {
            int count = 0;
            List<Vector2Int> adjacentPositions = GetAdjacentPositions(x, y);

            foreach (Vector2Int position in adjacentPositions)
            {
                if (_placedRooms.TryGetValue(position, out RoomType adjacentRoomType))
                {
                    if (adjacentRoomType == roomType)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public List<RoomType> GetConnectedChain(int startX, int startY)
        {
            List<RoomType> chainRooms = new List<RoomType>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            Vector2Int startPosition = new Vector2Int(startX, startY);

            if (!_placedRooms.ContainsKey(startPosition))
            {
                return chainRooms;
            }

            queue.Enqueue(startPosition);
            visited.Add(startPosition);

            while (queue.Count > 0)
            {
                Vector2Int currentPosition = queue.Dequeue();

                if (_placedRooms.TryGetValue(currentPosition, out RoomType roomType))
                {
                    chainRooms.Add(roomType);

                    List<Vector2Int> adjacentPositions = GetAdjacentPositions(currentPosition.x, currentPosition.y);

                    foreach (Vector2Int adjacentPosition in adjacentPositions)
                    {
                        if (!visited.Contains(adjacentPosition) && _placedRooms.ContainsKey(adjacentPosition))
                        {
                            visited.Add(adjacentPosition);
                            queue.Enqueue(adjacentPosition);
                        }
                    }
                }
            }

            return chainRooms;
        }

        public Vector2Int? FindRoomPosition(RoomType roomType)
        {
            foreach (KeyValuePair<Vector2Int, RoomType> placedRoom in _placedRooms)
            {
                if (placedRoom.Value == roomType)
                {
                    return placedRoom.Key;
                }
            }

            return null;
        }
    }
}
