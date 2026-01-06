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
        [SerializeField] private Sprite _roomIcon;
        [SerializeField] private string _roomName;
        [SerializeField] [TextArea(2, 4)] private string _description;
        [SerializeField] private List<RoomType> _connectsTo = new List<RoomType>();
        [SerializeField] private List<RoomType> _upgradesOtherRooms = new List<RoomType>();
        [SerializeField] private List<RoomType> _upgradedByOtherRooms = new List<RoomType>();
        [SerializeField] private List<RoomType> _effectRooms = new List<RoomType>();
        [SerializeField] private List<RoomCountRestriction> _countRestrictions = new List<RoomCountRestriction>();
        [SerializeField] private List<RoomChainExclusion> _chainExclusions = new List<RoomChainExclusion>();

        public RoomType RoomType => _roomType;
        public Sprite RoomIcon => _roomIcon;
        public string RoomName => _roomName;
        public string Description => _description;
        public List<RoomType> ConnectsTo => _connectsTo;
        public List<RoomType> UpgradesOtherRooms => _upgradesOtherRooms;
        public List<RoomType> UpgradedByOtherRooms => _upgradedByOtherRooms;
        public List<RoomType> EffectRooms => _effectRooms;
        public List<RoomCountRestriction> CountRestrictions => _countRestrictions;
        public List<RoomChainExclusion> ChainExclusions => _chainExclusions;
    }
}
