using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Grid
{
    public class GridCellView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _roomImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private TMP_Text _connectionCountText;

        [Header("Connection Lines")]
        [SerializeField] private Image _connectionLineNorth;
        [SerializeField] private Image _connectionLineSouth;
        [SerializeField] private Image _connectionLineEast;
        [SerializeField] private Image _connectionLineWest;

        public event Action<PointerEventData.InputButton> OnCellClicked;
        public event Action OnCellHoverEnter;
        public event Action OnCellHoverExit;

        private bool _hasRoom;

        public bool HasRoom => _hasRoom;
        public Image FrameImage => _frameImage;

        public void Initialize()
        {
            if (_roomImage != null)
            {
                Color color = _roomImage.color;
                color.a = 0f;
                _roomImage.color = color;
            }
        }

        public void SetCellName(int x, int y)
        {
            gameObject.name = $"[Image] GridCell ({x}, {y})";
        }

        public void PlaceRoom(Sprite roomIcon)
        {
            if (_hasRoom) return;

            _hasRoom = true;
            _roomImage.sprite = roomIcon;

            Color color = _roomImage.color;
            color.a = 1f;
            _roomImage.color = color;
        }

        public void EraseRoom()
        {
            if (!_hasRoom) return;

            _hasRoom = false;
            _roomImage.sprite = null;

            Color color = _roomImage.color;
            color.a = 0f;
            _roomImage.color = color;
        }

        public void SetConnectionCount(int count)
        {
            if (_connectionCountText == null) return;

            if (count > 0 && !_hasRoom)
            {
                _connectionCountText.text = count.ToString();
                _connectionCountText.gameObject.SetActive(true);
            }
            else
            {
                _connectionCountText.gameObject.SetActive(false);
            }
        }

        public void HideConnectionCount()
        {
            if (_connectionCountText == null) return;

            _connectionCountText.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnCellClicked?.Invoke(eventData.button);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnCellHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnCellHoverExit?.Invoke();
        }

        public void SetConnectionLineVisible(ConnectionDirection direction, bool visible)
        {
            Image connectionLine = GetConnectionLineImage(direction);

            if (connectionLine == null) return;

            connectionLine.gameObject.SetActive(visible);
        }

        public void HideAllConnectionLines()
        {
            SetConnectionLineVisible(ConnectionDirection.North, false);
            SetConnectionLineVisible(ConnectionDirection.South, false);
            SetConnectionLineVisible(ConnectionDirection.East, false);
            SetConnectionLineVisible(ConnectionDirection.West, false);
        }

        private Image GetConnectionLineImage(ConnectionDirection direction)
        {
            return direction switch
            {
                ConnectionDirection.North => _connectionLineNorth,
                ConnectionDirection.South => _connectionLineSouth,
                ConnectionDirection.East => _connectionLineEast,
                ConnectionDirection.West => _connectionLineWest,
                _ => null
            };
        }
    }
}