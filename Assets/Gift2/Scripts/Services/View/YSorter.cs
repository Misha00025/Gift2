using UnityEngine;

namespace Gift2
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YSorter : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private int _sortingOrder;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _sortingOrder = spriteRenderer.sortingOrder;
            spriteRenderer.sortingOrder = _sortingOrder + (int)(-transform.position.y);
        }

        void LateUpdate()
        {
            spriteRenderer.sortingOrder = _sortingOrder + (int)(-transform.position.y);
        }
    }
}
