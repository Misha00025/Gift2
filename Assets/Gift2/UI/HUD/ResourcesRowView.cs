using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace Gift2.Core
{
    public class ResourcesRowView : ResourcesView
    {
        [Header("Prefabs")]
        [SerializeField] private ResourceItemUI resourceItemPrefab; 
        [SerializeField] private GameObject floatingTextPrefab;

        [Header("Container")]
        [SerializeField] private RectTransform container;

        [Header("Colors")]
        [SerializeField] private Color increaseColor = Color.green;
        [SerializeField] private Color decreaseColor = Color.red;

        private Dictionary<string, ResourceItemUI> resourceItems = new Dictionary<string, ResourceItemUI>();

        protected override void OnInitialize()
        {
            if (ResourcesStorage == null)
            {
                Debug.LogError("ResourcesStorage is null!");
                return;
            }

            foreach (Transform child in container)
                Destroy(child.gameObject);
            resourceItems.Clear();

            foreach (var slot in ResourcesStorage.Slots)
            {
                if (slot.Item.Config == null) continue;

                
                var itemUI = Instantiate(resourceItemPrefab, container);

                itemUI.Initialize(slot);
                resourceItems[slot.Item.Key] = itemUI;
            }

            ResourcesStorage.OnSlotChanged += OnSlotChanged;
        }

        private void OnDestroy()
        {
            if (ResourcesStorage != null)
                ResourcesStorage.OnSlotChanged -= OnSlotChanged;
        }

        private void OnSlotChanged(IInventorySlot slot)
        {
            if (slot == null || slot.Item.Config == null) return;

            if (resourceItems.TryGetValue(slot.Item.Key, out ResourceItemUI itemUI))
            {
                int oldAmount = itemUI.CurrentAmount;
                itemUI.UpdateAmount(slot.Amount);

                int delta = slot.Amount - oldAmount;
                if (delta != 0)
                {
                    ShowFloatingText(slot.Item.Key, delta);
                }
            }
        }
        private void ShowFloatingText(string key, int delta)
        {
            if (floatingTextPrefab == null || !resourceItems.TryGetValue(key, out ResourceItemUI itemUI))
                return;

            Color color = delta > 0 ? increaseColor : decreaseColor;
            string sign = delta > 0 ? "+" : "";

            GameObject floatGO = Instantiate(floatingTextPrefab, itemUI.Container);
            TextMeshProUGUI tmp = floatGO.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = $"{sign}{delta}";
                tmp.color = color;
            }
            else
            {
                Text text = floatGO.GetComponent<Text>();
                if (text != null)
                {
                    text.text = $"{sign}{delta}";
                    text.color = color;
                }
            }

            
            Destroy(floatGO, 1f);
        }

        
        
    }
}