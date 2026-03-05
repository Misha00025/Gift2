using UnityEngine;

namespace Gift2.Core
{    
    public interface IItemConfig 
    {
        string Name { get; }
        Sprite Icon { get; }
        string Description { get; }
        int MaxStack { get; }
    }


    public struct Item
    {
        public string Key { get; private set; }
        public IItemConfig Config { get; private set; }
        public int MaxStack => Config.MaxStack;
        public bool Stackable => MaxStack > 1;
        
        public Item(string key, IItemConfig config)
        {
            Key = key;
            Config = config;
        }
    }
}