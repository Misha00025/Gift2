using Gift2.Core;
using UnityEngine;

public class ItemsCollector : MonoBehaviour
{
    private ResourcesStorage _storage;

    public void Initialize(ResourcesStorage storage)
    {
        _storage = storage;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CollectableItem>(out var collectable) && collectable.CanBeCollected)
        {
            var item = collectable.Item;
            var count = collectable.Amount;
            
            if (_storage.IsFull(item)) return;
            
            _storage.Add(item, count);
            Destroy(collectable.gameObject);
        }
    }
}
