using System;
using System.Collections.Generic;
using Gift2;
using Gift2.Core;
using UnityEngine;

public class ResourceContainer : Respawnble, IDamageable
{
    [Serializable]
    public struct Threshold
    {
        public int Health;
        public int Count;
        public CollectableItem Item;
    }

    public PropertyView HealthView;

    public List<Element> Weaknesses = new();
    public Property Health;
    public List<Threshold> Thresholds = new();
    
    public float MinStrength = 0.1f;
    
    private int _lastHealth;

    void Start()
    {
        HealthView?.SetProperty(Health);
        HealthView?.gameObject.SetActive(false);
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
        // for (int i = 0; i < count; i++)
        // {
            var item = Instantiate(prefab);
            item.Amount = count;
            item.transform.position = transform.position;
        // }
    }

    private int CalculateDamage(Damage damage)
    {
        if (Weaknesses.Contains(damage.Element) == false)
            damage.Strength -= 1f;
        if (MinStrength > damage.Strength)
            return 0;
        var result = damage.Value;
        result = Mathf.RoundToInt(result * damage.Strength);
        if (result <= 0)
            return 1;
        return result;
    }

    public void TakeDamage(Damage damage)
    {
        _lastHealth = Health.Value;
        
        Health.Value -= CalculateDamage(damage);
        
        if (Health.Value == 0)
            Kill();
        
    }

    protected override void OnRespawn()
    {
        Health.Value = Health.MaxValue;
        _lastHealth = Health.MaxValue;
    }
}
