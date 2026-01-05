using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoomConnectionData", menuName = "VaalTempleBuilder/Room Connection Data")]
    public class RoomConnectionData : ScriptableObject
    {
        [SerializeField] private List<RoomConnectionRule> _connectionRules = new List<RoomConnectionRule>();

        public List<RoomConnectionRule> ConnectionRules => _connectionRules;
    }
}
