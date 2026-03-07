using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

namespace Gift2
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopConfig _config;
        
        private Player _player;
        private List<ShopSlot> _slots = new();
        
        public IReadOnlyList<ShopSlot> Slots => _slots;
        
        void Start()
        {
            _player = FindAnyObjectByType<Player>();
            InitializeSlots();
        }
        
        private void InitializeSlots()
        {
            foreach (var slotConfig in _config.Slots)
            {
                _slots.Add(slotConfig.Build());
            }
        }
        
        public bool CanBuy(int slotIndex)
        {
            if (Slots.Count <= slotIndex || slotIndex < 0) return false;
            
            var ok = true;
            var slot = Slots[slotIndex];
            
            if (slot.CurrentBuy >= slot.MaxBuy) return false;
            
            for (int i = 0; i < slot.Costs.Count; i++)
            {
                var cost = slot.Costs[i];
                var playerAmount = _player.ResourcesStorage.Count(cost.Item);
                ok = ok && (cost.Amount <= playerAmount);
            }
            return ok;
        }
        
        private void TakeCost(ShopSlot slot)
        {
            var storage = _player.ResourcesStorage;
            
            foreach (var cost in slot.Costs)
            {
                var item = cost.Item;
                storage.Remove(item, cost.Amount);
            }
        }
        
        private void GiveUpgrade(ShopSlot slot)
        {
            _player.AddUpdate(slot.UpgradeModifiers);
        }
        
        private void GiveWeapon(ShopSlot slot)
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
                case ShopSlot.ProductType.Upgrade:
                    GiveUpgrade(slot);
                    break;
                case ShopSlot.ProductType.Weapon:
                    GiveWeapon(slot);
                    break;
            }
            slot.AddBuy();
        }        
    }
}
