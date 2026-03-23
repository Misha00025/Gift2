using UnityEngine;
using Wof.InventoryManagement;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class ItemsCollector : MonoBehaviour
{
    private Vector3 _defaultScale;
    [Inject] private IInventory _storage;
    [Inject] private Character _character;

    void Awake()
    {
        _defaultScale = transform.localScale;
    }
    
    void FixedUpdate()
    {
        if (_character == null) return;
        
        transform.localScale = _defaultScale * _character.Stats.CollectingRadiusScale;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<CollectableItem>(out var collectable))
        {
            var item = collectable.Item;
            if (_storage.IsFull(item)) return;
            
            if (collectable.CanBeCollected == false) 
            {
                collectable.CanCollectEvent.AddListener(OnCanCollect);
                return;
            }
            
            OnCanCollect(collectable);
        }
    }
    
    void OnCanCollect(CollectableItem collectable)
    {
        collectable.Collect(transform, OnCollect);
    }
    
    private bool OnCollect(CollectableItem collectable)
    {
        var item = collectable.Item;
        var count = collectable.Amount;
        
        if (_storage.IsFull(item)) return false;
        
        _storage.Add(item, count);
        return true;
    }
}
