using UnityEngine;

namespace Grid
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private Transform _cellContainer;

        public Transform CellContainer => _cellContainer;
    }
}