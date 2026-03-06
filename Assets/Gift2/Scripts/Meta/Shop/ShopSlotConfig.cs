using System;
using System.Collections.Generic;
using Gift2.Core;
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
    
        public enum ProductType 
        {
            Upgrade,
            Weapon
        }
    
        public string Name = "";
        public string Description = "";
        public Sprite Icon;
        public List<CostConfig> Costs = new();
        
        public ProductType Type;
        public Weapon WeaponPrefab;
        public CharacterStats UpgradeModifiers;
    }
}
