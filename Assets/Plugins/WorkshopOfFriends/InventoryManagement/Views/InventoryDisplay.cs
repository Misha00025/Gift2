using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


namespace Wof.InventoryManagement.UI
{
    public class InventoryDisplay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryConfig _testConfig;
        [SerializeField] private GameObject _slotPrefab;
        
        [Header("Layout Settings")]
        public int Columns = 5;
        public Vector2 Spacing = new Vector2(5, 5);
        public int Padding = 10;
        
        private Dictionary<int, InventorySlotUI> _slotUIs = new();
        private Inventory _inventory;
        private GridLayoutGroup _gridLayout;
        private RectTransform _rectTransform;
        
        private void Awake()
        {    
            _rectTransform = GetComponent<RectTransform>();
            _gridLayout = GetComponent<GridLayoutGroup>();
            if (_gridLayout == null)
                _gridLayout = gameObject.AddComponent<GridLayoutGroup>();
            
            _gridLayout.spacing = Spacing;
            _gridLayout.padding = new RectOffset(Padding, Padding, Padding, Padding);
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = Columns;
            _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            _gridLayout.childAlignment = TextAnchor.UpperLeft;
            if (_testConfig == null) return;
            Initialize(_testConfig.Build());
        }
        
        private void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.SlotChanged -= UpdateSlot;
                _inventory.OnInventoryChanged -= RefreshAllSlots;
            }
        }
        public void Initialize(Inventory targetInventory)
        {
            if (_inventory != null)
            {
                _inventory.SlotChanged -= UpdateSlot;
                _inventory.OnInventoryChanged -= RefreshAllSlots;
            }
            
            _inventory = targetInventory;
            
            if (_inventory == null) return;
            
            _inventory.SlotChanged += UpdateSlot;
            _inventory.OnInventoryChanged += RefreshAllSlots;
            
            CreateSlots(_inventory.Slots.Count);
            
            RefreshAllSlots();
            UpdateLayout();
        }
        
        private void CreateSlots(int slotCount)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            
            _slotUIs?.Clear();
            
            for (int i = 0; i < slotCount; i++)
            {
                GameObject slotGO = Instantiate(_slotPrefab, transform);
                slotGO.name = $"Slot_{i}";
                
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                if (slotUI == null)
                    slotUI = slotGO.AddComponent<InventorySlotUI>();
                
                slotUI.Index = i;
                slotUI.Inventory = _inventory; 
                
                _slotUIs[i] = slotUI;
            }
        }
        
        private void UpdateSlot(InventorySlot slot)
        {
            int index = System.Array.IndexOf(_inventory.Slots.ToArray(), slot);
            if (index >= 0 && _slotUIs.TryGetValue(index, out InventorySlotUI slotUI))
                slotUI.SetSlot(slot);
        }
        
        private void RefreshAllSlots()
        {
            if (_inventory == null) return;
            
            var slots = _inventory.Slots;
            for (int i = 0; i < slots.Count; i++)
            {
                if (_slotUIs.TryGetValue(i, out InventorySlotUI slotUI))
                    slotUI.SetSlot(slots[i]);
            }
        }
        
        private void RecalculateCellSize()
        {
            if (_gridLayout == null || _rectTransform == null) return;

            float availableWidth = _rectTransform.rect.width - Padding*2;

            float totalSpacing = Spacing.x * (Columns - 1);

            float cellWidth = (availableWidth - totalSpacing) / Columns;
            if (cellWidth < 1f) cellWidth = 1f;

            _gridLayout.cellSize = new Vector2(cellWidth, cellWidth);
        }
        
        public void UpdateLayout()
        {
            RecalculateCellSize();
        }
    }
}