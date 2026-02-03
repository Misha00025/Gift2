public interface IEffect 
{
    public void Apply(Character character);
    public void Disable();
}

public interface IOnHitEffect
{
    public void OnHit(Hit hit);
}

public interface IOnEffectApplyEffect
{
    public void OnEffectApply(IEffect effect);
}

public interface ITikableEffect
{
    public void OnTick();
}

public class Effect : IEffect
{
    protected Character Target { get; private set; }

    public virtual void Apply(Character character)
    {
        if (Target == null)
        {
            Target = character;
        }
    }
    
    public void Disable()
    {
        Target?.DisableEffect(this);
        Target = null;
    }
}

public class DurationEffect : Effect
{
    public float Duration { get; private set; }
    
    public DurationEffect(float duration)
    {
        Duration = duration;
    }

    public override void Apply(Character character)
    {
        EffectsRegister.Instance?.Register(this, Duration);
        base.Apply(character);
    }
}

public abstract class TikableEffect : DurationEffect, ITikableEffect
{
    protected TikableEffect(float duration = 1f) : base(duration)
    {
    }

    public abstract void OnTick();
}

public class CountedOnHitEffect : Effect, IOnHitEffect
{
    private int _count;

    public CountedOnHitEffect(int count = 1)
    {
        _count = count;
    }    

    public virtual void OnHit(Hit hit)
    {
        _count--;
        if (_count <= 0)
            Disable();
    }
}

public class OneHitEffect : CountedOnHitEffect {  }