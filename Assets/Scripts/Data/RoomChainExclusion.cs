using System;
using Enums;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RoomChainExclusion
    {
        [SerializeField] private RoomType _excludedRoomType;

        public RoomType ExcludedRoomType => _excludedRoomType;
    }
}
