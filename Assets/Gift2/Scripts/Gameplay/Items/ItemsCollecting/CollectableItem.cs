using Gift2.Core;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemConfig ItemConfig;
    [field: SerializeField] public int Amount = 1;
    public Item Item => ItemConfig.Build();
    
    public bool CanBeCollected { get; private set; } = true;
}
