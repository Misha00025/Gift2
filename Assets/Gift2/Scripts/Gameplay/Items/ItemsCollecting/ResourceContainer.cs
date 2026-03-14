using System;
using System.Collections.Generic;
using Gift2;
using Gift2.Core;
using UnityEngine;

public class ResourceContainer : Respawnble, IDamageable
{
    public enum DropTypeEnum
    {
        Thresholds,
        EveryDamage
    }

    [Serializable]
    public struct Threshold
    {
        public int Health;
        public int Count;
        public CollectableItem Item;
    }

    public string Key = "Resource";
    public PropertyView HealthView;

    public List<Element> Weaknesses = new();
    public Property Health;
    
    public DropTypeEnum DropType;
    public List<Threshold> Thresholds = new();
    public Threshold DamageToDrop;    
    
    
    public float MinStrength = 0.1f;
    
    private int _lastHealth;

    void Start()
    {
        HealthView?.SetProperty(Health);
        HealthView?.gameObject.SetActive(false);
        _lastHealth = Health.Value;
        if (DropType == DropTypeEnum.Thresholds)
        {
            foreach (var threshold in Thresholds)
            {
                Health.Changed.AddListener((health) => 
                {
                    if (threshold.Health >= _lastHealth || threshold.Health < health.Value) return;
                    DropItems(threshold.Item, threshold.Count);
                    _lastHealth = Health.Value;
                });
            }
        }
        else
        {
            Health.Changed.AddListener((health) => 
            {
                if (_lastHealth - health.Value > DamageToDrop.Health)
                {
                    _lastHealth -= DamageToDrop.Health;
                    DropItems(DamageToDrop.Item, DamageToDrop.Count);
                }
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
        var scale = DamageScaler.Instance?.GetScale(Key) ?? 1f;
        result = Mathf.RoundToInt(result * damage.Strength * scale);
        if (result <= 0)
            return 1;
        return result;
    }

    public void TakeDamage(Damage damage)
    {
        
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
