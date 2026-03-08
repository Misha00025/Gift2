using UnityEngine;
using UnityEngine.Events;

namespace Gift2
{
    public class ShopView : MonoBehaviour
    {
        public ShopController ShopController;
        public ShopSlotView SlotViewPrefab;
        public RectTransform Content;
        
        public UnityEvent Opened = new();
        public UnityEvent Closed = new();
        
        private ShopView Shop => this;
        
        
        void Start()
        {
            for (int i = 0; i < ShopController.Slots.Count; i++)
            {
                var view = Instantiate(SlotViewPrefab, Content);
                view.Initialize(ShopController, i);
            }
            gameObject.SetActive(false);
        }
        
        
        public void OpenShop()
        {
            Shop.gameObject.SetActive(true);
            Opened.Invoke();
        }
        
        public void CloseShop()
        {
            Closed.Invoke();
            Shop.gameObject.SetActive(false);
        }
    }
}
