using UnityEngine;
using Wof.InventoryManagement;

namespace Gift2.Gameplay
{
    [CreateAssetMenu(fileName = "BuffItem", menuName = "Inventory/Items/BuffItem")]
    public class BuffItemConfig : ItemConfig
    {
        public CharacterStats Multipliers;
    
        public override Item Build()
        {
            return new BuffItem(name, this, Multipliers);
        }
        
    }
}
