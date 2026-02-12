using UnityEngine;

public interface IEffect 
{
    public string Key { get; }
    public Sprite Icon { get; }

    public void Apply(Character character);
    public void Disable();
}

public interface IOnHitEffect
{
    public void OnHit(Hit hit);
}

public interface IOnHitAttemptEffect
{
    public void OnHitAttempt(Hit hit);
}

public interface IOnEffectApplyEffect
{
    public void OnEffectApply(IEffect effect);
}

public interface ITikableEffect
{
    public void OnTick();
}

public interface IBeforeTakeDamageEffect
{
    public void OnDamageTaking(ref Damage damage);
}

public class Effect : IEffect
{
    public string Key { get; set; } = "";
    public Sprite Icon { get; set; }
    protected Character Target { get; private set; }

    public virtual void Apply(Character character)
    {
        if (Target == null)
        {
            Target = character;
        }
    }
    
    public virtual void Disable()
    {
        Target?.DisableEffect(this);
        Target = null;
    }
}