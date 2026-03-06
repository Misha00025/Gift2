using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

namespace Gift2
{
    [CreateAssetMenu(fileName = "Shop", menuName = "Inventory/Shop")]
    public class ShopConfig : ScriptableObject
    {
        [SerializeField] private List<ShopSlotConfig> _slots = new();
        
        public IReadOnlyList<ShopSlotConfig> Slots => _slots;
    }
}
