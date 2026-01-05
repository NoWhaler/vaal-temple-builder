using System.Collections.Generic;
using Common;
using Enums;
using UnityEngine;
using VaalTempleBuilder;

namespace Medallions
{
    public class MedallionsView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<MedallionType, MedallionView> _medallions;

        public Dictionary<MedallionType, MedallionView> Medallions { get; private set; }

        public void Initialize()
        {
            _medallions.Initialize();
            Medallions = _medallions.GetDictionary();

            foreach (var medallion in Medallions.Values)
            {
                medallion.Initialize();
            }
        }

        public void Dispose()
        {
            foreach (var medallion in Medallions.Values)
            {
                medallion.Dispose();
            }
        }
    }
}