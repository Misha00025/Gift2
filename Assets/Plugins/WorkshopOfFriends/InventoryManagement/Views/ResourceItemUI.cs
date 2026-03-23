using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wof.InventoryManagement.UI
{
    public class ResourceItemUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _currentAmountText;
        [SerializeField] private TextMeshProUGUI _maxAmountText;
        public RectTransform Container;

        public int CurrentAmount { get; private set; }
        private int maxAmount;

        public void Initialize(IInventorySlot slot)
        {
            if (slot.Item.Config == null) return;

            if (_nameText != null)
                _nameText.SetText(slot.Item.Config.Name);

            _iconImage.sprite = slot.Item.Config.Icon;
            CurrentAmount = slot.Amount;
            maxAmount = slot.Item.MaxStack;

            UpdateTexts();
        }

        public void UpdateAmount(int newAmount)
        {
            CurrentAmount = newAmount;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            _currentAmountText.text = CurrentAmount.ToString();
            _maxAmountText.text = maxAmount.ToString();
        }
    }
}
    