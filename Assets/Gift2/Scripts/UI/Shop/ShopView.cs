using UnityEngine;

namespace Gift2
{
    public class ShopView : MonoBehaviour
    {
        public ShopController ShopController;
        public ShopSlotView SlotViewPrefab;
        public RectTransform Content;
        
        void Start()
        {
            for (int i = 0; i < ShopController.Slots.Count; i++)
            {
                var view = Instantiate(SlotViewPrefab, Content);
                view.Initialize(ShopController, i);
            }
        }
    }
}
