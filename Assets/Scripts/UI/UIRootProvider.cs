using UnityEngine;

namespace VaalTempleBuilder
{
    public class UIRootProvider : MonoBehaviour
    {
        [SerializeField] private Transform _uiRoot;

        public Transform UIRoot => _uiRoot;
    }
}