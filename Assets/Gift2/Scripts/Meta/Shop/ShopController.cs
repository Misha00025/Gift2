using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

namespace Gift2
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopConfig _config;
        
        private Player _player;
        
        public IReadOnlyList<ShopSlotConfig> Slots => _config.Slots;
        
        void Start()
        {
            _player = FindAnyObjectByType<Player>();
        }
        
        public bool CanBuy(int slotIndex)
        {
            if (Slots.Count <= slotIndex || slotIndex < 0) return false;
            
            var ok = true;
            var slot = Slots[slotIndex];
            for (int i = 0; i < slot.Costs.Count; i++)
            {
                var cost = slot.Costs[i];
                var playerAmount = _player.ResourcesStorage.Count(cost.Item.Build());
                ok = ok && (cost.Amount <= playerAmount);
            }
            return ok;
        }
        
        private void TakeCost(ShopSlotConfig slot)
        {
            var storage = _player.ResourcesStorage;
            
            foreach (var cost in slot.Costs)
            {
                var item = cost.Item.Build();
                storage.Remove(item, cost.Amount);
            }
        }
        
        private void GiveUpgrade(ShopSlotConfig slot)
        {
            _player.AddUpdate(slot.UpgradeModifiers);
        }
        
        private void GiveWeapon(ShopSlotConfig slot)
        {
            _player.Character.AddWeapon(slot.WeaponPrefab);
        }
        
        public void Buy(int slotIndex)
        {
            if (CanBuy(slotIndex) == false) return;
            
            var slot = Slots[slotIndex];
            TakeCost(slot);
            
            switch (slot.Type)
            {
                case ShopSlotConfig.ProductType.Upgrade:
                    GiveUpgrade(slot);
                    break;
                case ShopSlotConfig.ProductType.Weapon:
                    GiveWeapon(slot);
                    break;
            }
        }        
    }
}
