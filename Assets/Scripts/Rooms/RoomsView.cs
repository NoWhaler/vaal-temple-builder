using System.Collections.Generic;
using Common;
using Enums;
using UnityEngine;
using VaalTempleBuilder;

namespace Rooms
{
    public class RoomsView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<RoomType, RoomView> _rooms;

        public Dictionary<RoomType, RoomView> Rooms { get; private set; }

        public void Initialize()
        {
            _rooms.Initialize();
            Rooms = _rooms.GetDictionary();

            foreach (var room in Rooms.Values)
            {
                room.Initialize();
            }
        }

        public void Dispose()
        {
            foreach (var room in Rooms.Values)
            {
                room.Dispose();
            }
        }
    }
}