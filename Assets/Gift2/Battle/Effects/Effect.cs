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

public interface ITikableEffect : IEffect
{
    public float Duration { get; }
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

public abstract class TikableEffect : Effect, ITikableEffect
{
    public float Duration { get; private set; }
    
    public TikableEffect(float duration = 1f)
    {
        Duration = duration;
    }

    public override void Apply(Character character)
    {
        EffectsRegister.Instance?.Register(this);
        base.Apply(character);
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