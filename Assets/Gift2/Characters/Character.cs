using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour
{
    [SerializeField] private int _health;

    public int Health { 
        get => _health;
        private set
        {
            _health = value;
            HealthChanged.Invoke(_health);
        } 
    }
    
    [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<int> HealthChanged { get; private set; } = new();
    
    
    public virtual Damage CalculateDamage(Damage damage)
    {
        var result = damage;
        return result;
    }
    
    public virtual void ApplyDamage(Damage damage)
    {
        Health -= damage.Value;
        DamageTaken.Invoke(damage);
    }
}
