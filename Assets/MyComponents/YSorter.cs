using UnityEngine;
using UnityEngine.Rendering;

namespace Gift2
{
    public class YSorter : MonoBehaviour
    {
        private SortingGroup sortingGroup;
        private SpriteRenderer spriteRenderer;
        private int _groupSortingOrder;
        private int _sortingOrder;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            sortingGroup = GetComponent<SortingGroup>();
            if (spriteRenderer != null)
            {
                _sortingOrder = spriteRenderer.sortingOrder;
                spriteRenderer.sortingOrder = _sortingOrder + (int)(-transform.position.y);
            }
            if (sortingGroup != null)
            {
                _groupSortingOrder = sortingGroup.sortingOrder;
                sortingGroup.sortingOrder = _groupSortingOrder + (int)(-transform.position.y);
            }
            if (sortingGroup == null && spriteRenderer == null)
            {
                sortingGroup = gameObject.AddComponent<SortingGroup>();
                sortingGroup.sortingOrder = _groupSortingOrder + (int)(-transform.position.y);
            }
        }

        void LateUpdate()
        {
            if (sortingGroup != null)
                sortingGroup.sortingOrder = _groupSortingOrder + (int)(-transform.position.y);
            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = _sortingOrder + (int)(-transform.position.y);
        }
    }
}
