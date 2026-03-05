using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gift2.Core
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Config")]
    public class InventoryConfig : ScriptableObject
    {
        [Serializable]
        public class PreInitSlot
        {
            public ItemConfig Config;
            public int Amount;
        }
        
        public int SlotsCount = 20;
        public List<PreInitSlot> Items = new();
        
        public Inventory Build()
        {
            var inventory = new Inventory(SlotsCount);
            foreach (var slot in Items)
            {
                var item = slot.Config.Build();
                var count = slot.Amount;
                inventory.AddItem(item, count);
            }
            return inventory;
        }
    }
}
