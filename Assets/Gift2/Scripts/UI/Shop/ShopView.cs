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
        private TestInputs Inputs;
        
        
        void Start()
        {
            for (int i = 0; i < ShopController.Slots.Count; i++)
            {
                var view = Instantiate(SlotViewPrefab, Content);
                view.Initialize(ShopController, i);
            }
        }
        
        
        public void OpenShop()
        {
            Inputs = FindAnyObjectByType<TestInputs>();
            Shop.gameObject.SetActive(true);
            Inputs.Shop = this;
            Opened.Invoke();
        }
        
        public void CloseShop()
        {
            Inputs = FindAnyObjectByType<TestInputs>();
            Closed.Invoke();
            Inputs.Shop = null;
            Shop.gameObject.SetActive(false);
        }
    }
}
