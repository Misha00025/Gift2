using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class Character : MonoBehaviour
{
    
    [field: SerializeField] public Property Health { get; private set; }
    [field: SerializeField] public Property Shield { get; private set; }    
    [SerializeField]private Stats _stats = new(){attackSpeed = 1f, damage = 1};
    [field: SerializeField] public List<Element> Elements { get; private set; }
    
    
    [Header("Character Events")]
    [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Character> AttackCompleted { get; private set; } = new();
    
    
    [Header("View")]
    public Animator Animator;
    
    // Not inspected
    private Stats _baseStats;
    private List<IEffect> _effects = new();
    protected Character Target { get; private set; }
    
    
    public Stats BaseStats => _baseStats;
    public Stats Stats => _stats;
    public IReadOnlyList<IEffect> Effects => _effects;
    
    
    void Awake() { Init(); }
    
    protected virtual void Init()
    {
        _baseStats = _stats;
    }
    
    protected virtual void SetHealth(int newValue)
    {
        Health.Value = newValue;
    }
    
    public virtual void ApplyDamage(Damage damage)
    {
        SetHealth(Health.Value - damage.Value);
        DamageTaken.Invoke(damage);
    }
    
    public virtual void ApplyEffect(IEffect effect)
    {
        if (_effects.Contains(effect)) return;
        
        _effects.Add(effect);
        effect.Apply(this);
    }
    
    public void DisableEffect(IEffect effect)
    {
        if (!_effects.Contains(effect)) return;
        
        _effects.Remove(effect);
        effect.Disable();
    }
    
    protected virtual Hit PrepareHit(Damage damage)
    {
        var hit = new Hit(){ Target = Target, Damage = damage };
        foreach (var effect in _effects)
        {
            if (effect is IOnHitEffect)
                hit.Applied += ((IOnHitEffect)effect).OnHit;
        }
        return hit;
    }
    
    public virtual void CompleteAttack() => AttackCompleted.Invoke(this);
    
    public void SetTarget(Character target)
    {
        Target = target;
    }
    
    public abstract void Attack();
}
