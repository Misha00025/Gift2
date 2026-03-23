using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wof.InventoryManagement
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
                inventory.Add(item, count);
            }
            return inventory;
        }
    }
}
