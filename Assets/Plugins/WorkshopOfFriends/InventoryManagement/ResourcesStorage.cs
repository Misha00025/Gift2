using System;
using System.Collections.Generic;
using System.Linq;

namespace Wof.InventoryManagement
{
    public class ResourcesStorage
    {
        private InventorySlot[] _resources;
        private readonly Dictionary<string, int> _indexByKey;
        
        public event Action<IInventorySlot> OnSlotChanged;
        
        public ResourcesStorage(IReadOnlyList<InventorySlot> slots)
        {
            _resources = new InventorySlot[slots.Count];
            _indexByKey = new Dictionary<string, int>(slots.Count);
            
            for (var i = 0; i < slots.Count; i++)
            {
                _resources[i] = slots[i];
                _indexByKey[slots[i].Item.Key] = i;
            }
        }

        public ResourcesStorage(IReadOnlyList<Item> resources)
        {
            _resources = new InventorySlot[resources.Count()];
            _indexByKey = new Dictionary<string, int>(resources.Count);
            
            for (int i = 0; i < _resources.Length; i++)
            {
                var item = resources[i];
                _resources[i] = new(item, 0);
                _indexByKey[item.Key] = i;
            }
        }
        
        public IReadOnlyList<IInventorySlot> Slots => _resources;
        
        public bool Add(Item item, int amount = 1) => Add(item.Key, amount);
        public bool Add(string key, int amount = 1)
        {
            if (string.IsNullOrEmpty(key) || amount <= 0)
                return false;

            if (!_indexByKey.TryGetValue(key, out int index))
                return false;

            var slot = _resources[index];
            slot.Amount += amount;
            if (slot.Amount > slot.Item.Config.MaxStack)
                slot.Amount = slot.Item.Config.MaxStack;
            OnSlotChanged?.Invoke(slot);
            return true;
        }
        
        public bool Remove(Item item, int amount = 1) => Remove(item.Key, amount);        
        public bool Remove(string key, int amount = 1)
        {
            if (string.IsNullOrEmpty(key) || amount <= 0)
                return false;

            if (!_indexByKey.TryGetValue(key, out int index))
                return false;

            var slot = _resources[index];
            if (slot.Amount < amount)
                return false;

            slot.Amount -= amount;
            OnSlotChanged?.Invoke(slot);
            return true;
        }
        
        public int Count(Item item) => Count(item.Key);
        public int Count(string key)
        {
            if (string.IsNullOrEmpty(key) || !_indexByKey.TryGetValue(key, out int index))
                return 0;

            return _resources[index].Amount;
        }
        
        public bool HasEnough(Item item, int requiredAmount) => HasEnough(item.Key, requiredAmount);
        public bool HasEnough(string key, int requiredAmount)
        {
            return Count(key) >= requiredAmount;
        }
        
        public bool IsFull(Item item) => IsFull(item.Key);
        public bool IsFull(string key)
        {
            if (string.IsNullOrEmpty(key) || !_indexByKey.TryGetValue(key, out int index))
                return false;
                
            return Count(key) == Slots[index].MaxAmount;
        }
        
        public void Clear()
        {
            for (int i = 0; i < _resources.Length; i++)
            {
                if (_resources[i].Amount != 0)
                {
                    _resources[i].Amount = 0;
                    OnSlotChanged?.Invoke(_resources[i]);
                }
            }
        }
    }
}
