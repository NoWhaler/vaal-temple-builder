using System;
using Enums;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RoomCountRestriction
    {
        [SerializeField] private RoomType _restrictedRoomType;
        [SerializeField] private int _maxAdjacentCount;

        public RoomType RestrictedRoomType => _restrictedRoomType;
        public int MaxAdjacentCount => _maxAdjacentCount;
    }
}
