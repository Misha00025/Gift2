using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wof.InventoryManagement
{
    [CreateAssetMenu(fileName = "ResourcesStorage", menuName = "Inventory/ResourcesStorage")]
    public class ResourcesStorageConfig : ScriptableObject
    {
        [Serializable]
        public struct ItemSlot
        {
            public ItemConfig Config;
            public bool OverrideMaxAmount;
            public int NewMaxAmount;
        }

        public List<ItemSlot> Resources = new();
    
        public ResourcesStorage Build()
        {
            return new(Resources.Select(e => {
                var item = e.Config.Build();
                var slot = new InventorySlot(item, 0);
                if (e.OverrideMaxAmount)
                    slot.MaxAmount = e.NewMaxAmount;
                return slot;
            }).ToList());
        }
    }
}
