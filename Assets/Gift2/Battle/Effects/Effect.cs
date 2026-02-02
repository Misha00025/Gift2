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

public class Effect : IEffect
{
    protected Character Target { get; private set; }

    public void Apply(Character character)
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

public class OneHitEffect : CountedOnHitEffect
{

}