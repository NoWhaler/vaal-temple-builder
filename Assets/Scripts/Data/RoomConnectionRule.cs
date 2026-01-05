using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RoomConnectionRule
    {
        [SerializeField] private RoomType _roomType;
        [SerializeField] private List<RoomType> _connectsTo = new List<RoomType>();
        [SerializeField] private List<RoomCountRestriction> _countRestrictions = new List<RoomCountRestriction>();
        [SerializeField] private List<RoomChainExclusion> _chainExclusions = new List<RoomChainExclusion>();

        public RoomType RoomType => _roomType;
        public List<RoomType> ConnectsTo => _connectsTo;
        public List<RoomCountRestriction> CountRestrictions => _countRestrictions;
        public List<RoomChainExclusion> ChainExclusions => _chainExclusions;
    }
}
