using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gift2.Core
{
    [CreateAssetMenu(fileName = "new ResourcesStorage", menuName = "Inventory/ResourcesStorage")]
    public class ResourcesStorageConfig : ScriptableObject
    {
        public List<ItemConfig> Resources = new();
    
        public ResourcesStorage Build()
        {
            return new(Resources.Select(e => e.Build()).ToList());
        }
    }
}
