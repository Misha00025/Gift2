using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gift2
{
    public class ShopSlotView : MonoBehaviour
    {
        public Image Icon;
        public TextMeshProUGUI NameField;
        public TextMeshProUGUI DescriptionField;
        public Transform CostsContainer;
        public Button BuyButton;
        public CostView CostViewPrefab;
        
        private ShopController _controller;
        private int _index;
        private List<CostView> _costViews = new();
        
        
        public void Initialize(ShopController controller, int slotIndex)
        {
            _controller = controller;
            _index = slotIndex;
            
            BuyButton.onClick.AddListener(Buy);
            ReloadView();
            ReloadCosts();
        }
        
        private void Buy()
        {
            if (_controller.CanBuy(_index) == false) return;
              
            _controller.Buy(_index);
            ReloadCosts();
        }
        
        private void ReloadView()
        {
            var slot = _controller.Slots[_index];
            Icon.sprite = slot.Icon;
            NameField.SetText(slot.Name);
            DescriptionField.SetText(slot.Description);
            
        }
        
        private void ReloadCosts()
        {
            var slot = _controller.Slots[_index];
            
            for (int i = 0; i < slot.Costs.Count; i++)
            {
                if (_costViews.Count <= i)
                    _costViews.Add(Instantiate(CostViewPrefab, CostsContainer));
                
                var cost = slot.Costs[i];
                var view = _costViews[i];
                view.Amount.SetText(cost.Amount.ToString());
                view.Icon.sprite = cost.Item.Icon;
            }
        }
    }
}
