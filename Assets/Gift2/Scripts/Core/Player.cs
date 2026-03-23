using System.Collections.Generic;
using Gift2.Meta;
using UnityEngine;
using Wof.InventoryManagement;

namespace Gift2.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private InventoryConfig DefaultInventory;
        [SerializeField] private ResourcesStorageConfig DefaultResources;
    
        private Inventory _inventory;
        private ResourcesStorage _resourcesStorage;
        
        public Character Character;
    
        public void Initialize()
        {
            if (DefaultInventory != null)
                _inventory = DefaultInventory.Build();
            if (DefaultResources != null)
                _resourcesStorage = DefaultResources.Build();
        }
    
        public void Initialize(Inventory inventory, ResourcesStorage resourcesStorage)
        {
            _inventory = inventory;
            _resourcesStorage = resourcesStorage;
        }
        
        public Inventory Inventory => _inventory;
        public ResourcesStorage ResourcesStorage => _resourcesStorage;
        public QuestsManager QuestsManager => QuestsManager.Instance;
        public IReadOnlyList<IInventorySlot> Items => _inventory?.Slots;
        public IReadOnlyList<IInventorySlot> Resources => _resourcesStorage?.Slots;
        
        public void AddUpdate(CharacterStats update) => Character?.AddUpdate(update);
    }
}
