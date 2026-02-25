using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gift2.Core
{
    public class ResourceItemUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI currentText;
        [SerializeField] private TextMeshProUGUI maxText;
        public RectTransform Container;

        public int CurrentAmount { get; private set; }
        private int maxAmount;

        public void Initialize(IInventorySlot slot)
        {
            if (slot.Item.Config == null) return;

            iconImage.sprite = slot.Item.Config.Icon;
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
            currentText.text = CurrentAmount.ToString();
            maxText.text = maxAmount.ToString();
        }
    }
}
    