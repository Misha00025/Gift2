using System;
using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

public class ResourceContainer : MonoBehaviour, IDamageable
{
    [Serializable]
    public struct Threshold
    {
        public int Health;
        public int Count;
        public CollectableItem Item;
    }

    public PropertyView HealthView;


    public Property Health;
    public List<Threshold> Thresholds = new();
    
    private int _lastHealth;

    void Start()
    {
        HealthView?.SetProperty(Health);
        _lastHealth = Health.Value;
        foreach (var threshold in Thresholds)
        {
            Health.Changed.AddListener((health) => 
            {
               if (threshold.Health >= _lastHealth || threshold.Health < health.Value) return;
               
               DropItems(threshold.Item, threshold.Count);
            });
        }
    }

    private void DropItems(CollectableItem prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(prefab);
            item.transform.position = transform.position;
        }
    }

    public void TakeDamage(Damage damage)
    {
        _lastHealth = Health.Value;
        Health.Value -= damage.Value;
        
        if (Health.Value == 0)
            Destroy(gameObject);
    }
    
    
}
