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
        Debug.Log("Trigger Enter!");
        if (collision.TryGetComponent<CollectableItem>(out var collectable) && collectable.CanBeCollected)
        {
            var item = collectable.Item;
            var count = collectable.Amount;
                        
            _storage.Add(item, count);
            Destroy(collectable.gameObject);
        }
    }
}
