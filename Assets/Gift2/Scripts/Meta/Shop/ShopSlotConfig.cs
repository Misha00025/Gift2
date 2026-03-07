using System;
using System.Collections.Generic;
using System.Linq;
using Gift2.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Gift2
{
    [Serializable]
    public class ShopSlotConfig
    {
        [Serializable]
        public class CostConfig
        {
            public ItemConfig Item;
            public int Amount = 1;
        }
    
        [Serializable]
        public class CostThreshold
        {
            public List<CostConfig> Costs = new();
        }
    
        public string Name = "";
        public string Description = "";
        public Sprite Icon;
        public int MaxBuy => CostsOnBuy.Count;
        public List<CostThreshold> CostsOnBuy = new();
        
        public ShopSlot.ProductType Type;
        public Weapon WeaponPrefab;
        public CharacterStats UpgradeModifiers;
        
        public ShopSlot Build()
        {
            return new ShopSlot(this);
        }
    }
    
    public class ShopSlot
    {
        public class Cost
        {
            public Sprite Icon;
            public Item Item;
            public int Amount = 1;
        }
        
        public enum ProductType 
        {
            Upgrade,
            Weapon
        }
    
        private ShopSlotConfig _config;
    
        public string Name => _config.Name;
        public string Description => _config.Description;
        public Sprite Icon => _config.Icon;
        
        public int CurrentBuy { get; private set; } = 0;
        public int MaxBuy => _config.MaxBuy;
        
        
        public IReadOnlyList<Cost> Costs => GetCosts();
        
        public ProductType Type => _config.Type;
        public Weapon WeaponPrefab => _config.WeaponPrefab;
        public CharacterStats UpgradeModifiers => _config.UpgradeModifiers;
    
        public ShopSlot(ShopSlotConfig config)
        {
            _config = config;
        }
        
        public void AddBuy()
        {
            if(CurrentBuy < MaxBuy)
                CurrentBuy++;
        }
        
        private List<Cost> GetCosts()
        {
            if (CurrentBuy >= MaxBuy) return new();
            
            var costConfigs = _config.CostsOnBuy[CurrentBuy].Costs;
            return costConfigs
                .Select(e => new Cost()
                {
                    Icon = e.Item.Icon, 
                    Item = e.Item.Build(), 
                    Amount = e.Amount
                })
                .ToList();
        }
    }
}
