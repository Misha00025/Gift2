using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Wof.InventoryManagement.UI
{
    public class ResourcesListView : ResourcesView
    {
        [Header("Prefabs")]
        [SerializeField] private ResourceItemUI resourceItemPrefab; 
        [SerializeField] private GameObject floatingTextPrefab;

        [Header("Container")]
        [SerializeField] private RectTransform container;

        [Header("Colors")]
        [SerializeField] private Color increaseColor = Color.green;
        [SerializeField] private Color decreaseColor = Color.red;
        
        [Header("Buffer")]
        [SerializeField] private float _timeToDelete = 1f;

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


            if (_buffer.ContainsKey(key) == false)
            {
                GameObject floatGO = Instantiate(floatingTextPrefab, itemUI.Container);
                TextMeshProUGUI view = floatGO.GetComponent<TextMeshProUGUI>();
                _buffer.Add(key, new()
                {
                   Amount = 0,
                   AccumulatedTime = 0f,
                   View = view
                });
            }
            var buffer = _buffer[key];
            var tmp = buffer.View;
            buffer.Amount += delta;
            buffer.AccumulatedTime = 0f;
            Color color = buffer.Amount > 0 ? increaseColor : decreaseColor;
            string sign = buffer.Amount > 0 ? "+" : "";
            
            if (tmp != null)
            {
                tmp.text = $"{sign}{buffer.Amount}";
                tmp.color = color;
            }
        }
        
        private class ItemBuffer
        {
            public int Amount;
            public float AccumulatedTime;
            public TextMeshProUGUI View;
        }

        private Dictionary<string, ItemBuffer> _buffer = new();
        
        void LateUpdate()
        {
            var toRemove = new List<string>();
            
            foreach (var key in _buffer.Keys)
            {
                var buffer = _buffer[key];
                buffer.AccumulatedTime += Time.deltaTime;
                if (buffer.AccumulatedTime > _timeToDelete)
                {
                    Destroy(buffer.View.gameObject);
                    toRemove.Add(key);
                }
            }
            
            foreach (var key in toRemove)
                _buffer.Remove(key);
        }
    }
}