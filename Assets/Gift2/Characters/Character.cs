using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CharacterViewController))]
public abstract class Character : MonoBehaviour
{
    
    [field: SerializeField] public Property Health { get; private set; }
    [SerializeField]private Stats _stats = new(){attackSpeed = 1f, damage = 1};
    [field: SerializeField] public List<Element> Elements { get; private set; }
    [field: SerializeField] public Skill MainActiveSkill { get; private set; }
    [field: SerializeField] public Skill SupportActiveSkill { get; private set; }
    
    
    [Header("Character Events")]
    [field: SerializeField] public UnityEvent<Damage> DamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Character> AttackCompleted { get; private set; } = new();
    
    
    [Header("View")]
    private CharacterViewController _view;
    public CharacterViewController View 
    {
        get 
        {
            if (_view == null)
                _view = GetComponent<CharacterViewController>();
            return _view;
        }
    }    
    public Animator Animator => _view.Animator;
    public Transform Pivot => _view.Pivot;
    
    // Not inspected
    private Stats _baseStats;
    private List<IEffect> _effects = new();
    private Dictionary<string, EffectView> _effectsViews = new();
    public Character Target { get; private set; }
    
    
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
        for (var i = _effects.Count - 1; i >= 0; i--)
        {   
            var effect = _effects[i];
            if (effect is IBeforeTakeDamageEffect)
                ((IBeforeTakeDamageEffect)effect).OnDamageTaking(ref damage);
        }
    
        SetHealth(Health.Value - damage.Value);
        DamageTaken.Invoke(damage);
    }
    
    public virtual void ApplyEffect(IEffect effect)
    {
        if (_effects.Contains(effect)) return;
        
        _effects.Add(effect);
        effect.Apply(this);
        
        if (_effectsViews.ContainsKey(effect.Key))
        {
            _effectsViews[effect.Key].gameObject.SetActive(true);
        }
        else
        {
            var view = EffectConfigsRegister.Instance.CreateView(effect.Key, this);
            if (view != null)
                _effectsViews.Add(effect.Key, view);
        }
    }
    
    public virtual void BaseAttack()
    {
        if (Target == null) return;
        
        var hit = PrepareHit(new Damage(){ Value = Stats.damage, Element = Elements.Count > 0 ? Elements[0] : Element.Physical});
        hit.Apply();
    }
    
    public void DisableEffect(IEffect effect)
    {
        if (!_effects.Contains(effect)) return;
        
        _effects.Remove(effect);
        effect.Disable();
        
        if (!_effects.Any(e => e.Key == effect.Key) && _effectsViews.ContainsKey(effect.Key))
        {
            _effectsViews[effect.Key].gameObject.SetActive(false);
        }
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
