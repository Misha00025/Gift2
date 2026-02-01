using UnityEngine;
using UnityEngine.Events;


public abstract class Character : MonoBehaviour
{
    [SerializeField]private Stats _stats = new(){attackSpeed = 1f, damage = 1};
    private Stats _baseStats;
    
    public Stats BaseStats => _baseStats;
    public Stats Stats => _stats;
    
    public Animator Animator;

    [field: SerializeField] public Property Health { get; private set; }
    [field: SerializeField] public Property Shield { get; private set; }
    
    [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Character> AttackCompleted { get; private set; } = new();
    
    void Awake() { Init(); }
    
    protected virtual void Init()
    {
        _baseStats = _stats;
    } 
    
    protected virtual void SetHealth(int newValue)
    {
        Health.Value = newValue;
    }
    
    public virtual Damage CalculateDamage(Damage damage)
    {
        var result = damage;
        return result;
    }
    
    public virtual void ApplyDamage(Damage damage)
    {
        SetHealth(Health.Value - damage.Value);
        DamageTaken.Invoke(damage);
    }
    
    public virtual void CompleteAttack() => AttackCompleted.Invoke(this);
    public abstract void Attack(Character target);
}
