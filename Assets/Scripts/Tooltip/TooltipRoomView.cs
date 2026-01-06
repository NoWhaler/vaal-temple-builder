using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace VaalTempleBuilder
{
    public class TooltipRoomView : MonoBehaviour
    {
        [SerializeField] private Image _roomIcon;

        public void SetRoomIcon(Sprite icon)
        {
            if (_roomIcon != null)
            {
                _roomIcon.sprite = icon;
            }
        }
    }
}