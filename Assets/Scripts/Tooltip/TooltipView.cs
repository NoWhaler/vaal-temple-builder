using TMPro;
using UnityEngine;

namespace VaalTempleBuilder
{
    public class TooltipView : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private Transform _headerContainer;
        [SerializeField] private Transform _descriptionContainer;
        [SerializeField] private Transform _roomConnectionsContainer;
        [SerializeField] private Transform _upgradeOtherRoomsContainer;
        [SerializeField] private Transform _upgradedByOtherRoomsContainer;
        [SerializeField] private Transform _effectRoomsContainer;

        [Header("Text Elements")]
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _descriptionText;

        [Header("Components")]
        [SerializeField] private RectTransform _tooltipRectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        public RectTransform TooltipRectTransform => _tooltipRectTransform;
        public Transform RoomConnectionsContainer => _roomConnectionsContainer;
        public Transform UpgradeOtherRoomsContainer => _upgradeOtherRoomsContainer;
        public Transform UpgradedByOtherRoomsContainer => _upgradedByOtherRoomsContainer;
        public Transform EffectRoomsContainer => _effectRoomsContainer;

        public void Initialize()
        {
            Hide();
        }

        public void Show()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void SetRoomName(string roomName)
        {
            if (_roomNameText != null)
            {
                _roomNameText.text = roomName;
            }
        }

        public void SetDescription(string description)
        {
            if (_descriptionText != null)
            {
                _descriptionText.text = description;
            }
        }

        public void SetPosition(Vector3 position)
        {
            if (_tooltipRectTransform != null)
            {
                _tooltipRectTransform.position = position;
            }
        }

        public void ClearContainer(Transform container)
        {
            if (container == null) return;

            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetContainerVisible(Transform container, bool visible)
        {
            if (container != null)
            {
                container.gameObject.SetActive(visible);
            }
        }

        public void Dispose()
        {
            ClearContainer(_roomConnectionsContainer);
            ClearContainer(_upgradeOtherRoomsContainer);
            ClearContainer(_upgradedByOtherRoomsContainer);
            ClearContainer(_effectRoomsContainer);
        }
    }
}
