using UnityEngine;

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
}
