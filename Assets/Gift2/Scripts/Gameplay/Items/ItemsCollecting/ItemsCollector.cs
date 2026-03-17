using Gift2.Core;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemsCollector : MonoBehaviour
{
    private Vector3 _defaultScale;
    private ResourcesStorage _storage;
    private Character _character;

    void Awake()
    {
        _defaultScale = transform.localScale;
    }

    public void SetStorage(ResourcesStorage storage)
    {
        _storage = storage;
    }
    
    public void Initialize(Character character)
    {
        _character = character;
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
        var item = collectable.Item;
        var count = collectable.Amount;
        
        if (_storage.IsFull(item)) return;
        
        _storage.Add(item, count);
        collectable.Collect();
    }
}
