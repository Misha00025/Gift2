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
        
        private ShopController _controller;
        private int _index;
        
        
        public void Initialize(ShopController controller, int slotIndex)
        {
            _controller = controller;
            _index = slotIndex;
            
            BuyButton.onClick.AddListener(Buy);
            ReloadView();
        }
        
        private void Buy()
        {
            _controller.Buy(_index);
            ReloadView();
        }
        
        private void ReloadView()
        {
            var slot = _controller.Slots[_index];
            Icon.sprite = slot.Icon;
            NameField.SetText(slot.Name);
            DescriptionField.SetText(slot.Description);
            
        }
    }
}
