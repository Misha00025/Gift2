using UnityEngine;
using System;
using System.Collections.Generic;


namespace Wof.InventoryManagement
{    
    public interface IInventorySlot
    {
        public Item Item { get; }
        public int Amount { get; }
        public int MaxAmount { get; }
        public bool IsEmpty { get; }
        public bool IsFull { get; }
    }

    public class InventorySlot : IInventorySlot
    {
        public Item Item { get; set; }
        public int Amount { get; set; }
        public int MaxAmount { get; set; }

        public InventorySlot()
        {
            Item = default;
            Amount = 0;
        }

        public InventorySlot(Item item, int amount)
        {
            this.Item = item;
            this.Amount = amount;
            this.MaxAmount = item.MaxStack;
        }

        public bool IsEmpty => Item?.Config == null || Amount == 0;
        public bool IsFull => !IsEmpty && Amount >= Item.MaxStack;

        public void Clear()
        {
            Item = default;
            Amount = 0;
        }
    }

    public class Inventory
    {
        private int _slotCount;
        private List<InventorySlot> _slots;

        public event Action<InventorySlot> SlotChanged;
        public event Action OnInventoryChanged;

        public Inventory(int slotCount)
        {
            _slotCount = slotCount;
            _slots = new();
            for (int i = 0; i < slotCount; i++)
                _slots.Add(new InventorySlot());
        }

        public IReadOnlyList<InventorySlot> Slots => _slots;

        public bool AddItem(Item item, int amount = 1)
        {
            if (item.Config == null || amount <= 0) return false; 

            int remaining = amount;

            if (item.Stackable)
            {
                for (int i = 0; i < _slots.Count && remaining > 0; i++)
                {
                    if (!_slots[i].IsEmpty && _slots[i].Item.Key == item.Key && !_slots[i].IsFull)
                    {
                        int freeSpace = item.MaxStack - _slots[i].Amount;
                        int toAdd = Mathf.Min(freeSpace, remaining);
                        _slots[i].Amount += toAdd;
                        remaining -= toAdd;
                        NotifySlotChanged(i);
                    }
                }
            }

            if (remaining > 0)
            {
                for (int i = 0; i < _slots.Count && remaining > 0; i++)
                {
                    if (_slots[i].IsEmpty)
                    {
                        int toAdd = item.Stackable ? Mathf.Min(item.MaxStack, remaining) : 1;
                        _slots[i].Item = item;      
                        _slots[i].Amount = toAdd;
                        remaining -= toAdd;
                        NotifySlotChanged(i);
                    }
                }
            }

            bool success = remaining < amount;
            if (success) NotifyInventoryChanged();
            return success;
        }

        public bool RemoveItem(string key, int amount = 1)
        {
            if (string.IsNullOrEmpty(key) || amount <= 0) return false;

            int remaining = amount;

            for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
            {
                if (!_slots[i].IsEmpty && _slots[i].Item.Key == key)
                {
                    int toRemove = Mathf.Min(_slots[i].Amount, remaining);
                    _slots[i].Amount -= toRemove;
                    remaining -= toRemove;
                    if (_slots[i].Amount == 0)
                        _slots[i].Item = default;
                    NotifySlotChanged(i);
                }
            }

            bool success = remaining < amount;
            if (success) NotifyInventoryChanged();
            return success;
        }

        public bool RemoveFromSlot(int index, int amount = 1)
        {
            if (index < 0 || index >= _slots.Count) return false;
            if (_slots[index].IsEmpty) return false;

            int toRemove = Mathf.Min(_slots[index].Amount, amount);
            _slots[index].Amount -= toRemove;
            if (_slots[index].Amount == 0)
                _slots[index].Item = default;

            NotifySlotChanged(index);
            NotifyInventoryChanged();
            return true;
        }

        public int CountItem(string key)
        {
            int total = 0;
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty && slot.Item.Key == key)
                    total += slot.Amount;
            }
            return total;
        }

        // Обмен слотов
        public void SwapSlots(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= _slots.Count || indexB < 0 || indexB >= _slots.Count)
                return;

            InventorySlot temp = new InventorySlot(_slots[indexA].Item, _slots[indexA].Amount);
            _slots[indexA].Item = _slots[indexB].Item;
            _slots[indexA].Amount = _slots[indexB].Amount;
            _slots[indexB].Item = temp.Item;
            _slots[indexB].Amount = temp.Amount;

            NotifySlotChanged(indexA);
            NotifySlotChanged(indexB);
            NotifyInventoryChanged();
        }

        // Очистка
        public void Clear()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].Clear();
                NotifySlotChanged(i);
            }
            NotifyInventoryChanged();
        }

        private void NotifySlotChanged(int index)
        {
            SlotChanged?.Invoke(_slots[index]);
        }

        private void NotifyInventoryChanged()
        {
            OnInventoryChanged?.Invoke();
        }
    }
}