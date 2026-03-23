using UnityEngine;
using Wof.InventoryManagement;

namespace Gift2.Gameplay
{
    public class BuffItem : Item, IBuff
    {
        private CharacterStats _multipliers;
    
        public BuffItem(string key, IItemConfig config, CharacterStats multipliers) : base(key, config)
        {
            _multipliers = multipliers;
        }

        public CharacterStats Apply(CharacterStats stats)
        {
            stats.RotationSpeed += _multipliers.RotationSpeed;
            stats.RotationRadius += _multipliers.RotationRadius;
            stats.MoveSpeed += _multipliers.MoveSpeed;
            stats.CollectingRadiusScale += _multipliers.CollectingRadiusScale;
            stats.Strength += _multipliers.Strength;
            return stats;
        }
    }

    public interface IBuff 
    {
        public CharacterStats Apply(CharacterStats stats);
    }
}
