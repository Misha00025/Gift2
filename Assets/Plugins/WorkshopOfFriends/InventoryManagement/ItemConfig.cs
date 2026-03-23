using UnityEngine;

namespace Wof.InventoryManagement
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class ItemConfig : ScriptableObject, IItemConfig
    {
        [field: SerializeField] public string Name { get; set; } = "New Item";
        [field: SerializeField] public Sprite Icon { get; set; } = null;
        [field: SerializeField, TextArea] public string Description { get; set; } = "";
        [field: SerializeField] public int MaxStack { get; set; } = 1;
        
        public virtual Item Build()
        {
            return new (name, this);
        }
    }
}