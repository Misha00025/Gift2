using UnityEngine;
using UnityEngine.Events;


public abstract class Character : MonoBehaviour
{
    [SerializeField] private int _health;
    
    public Animator Animator;

    public int Health { 
        get => _health;
        protected set => SetHealth(value);
    }
    
    [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<int> HealthChanged { get; private set; } = new();
    
    
    public abstract void Attack(Character target); 
    
    protected virtual void SetHealth(int newValue)
    {
        _health = newValue;
        HealthChanged.Invoke(_health);
    }
    
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
