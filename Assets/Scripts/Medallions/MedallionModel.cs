using Enums;
using UnityEngine;

namespace Medallions
{
    public class MedallionModel
    {
        public MedallionType MedallionType { get; private set; }
        public Sprite Icon { get; private set; }

        public MedallionModel(MedallionType medallionType, Sprite icon)
        {
            MedallionType = medallionType;
            Icon = icon;
        }
    }
}