using System.Collections.Generic;
using UnityEngine;

namespace Gift2.Core
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private InventoryConfig DefaultInventory; 
    
        private Inventory _inventory;
        
        void Awake()
        {
            if (DefaultInventory != null)
                _inventory = DefaultInventory.Build();
        }
    
        public void Initialize(Inventory inventory)
        {
            _inventory = inventory;
        }
        
        public IReadOnlyList<IInventorySlot> Slots => _inventory.Slots;
    }
}
