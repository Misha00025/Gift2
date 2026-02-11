using System;

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

public class TikableCompositeEffect : TikableEffect
{
    private Action<Character> _action;
    
    public TikableCompositeEffect(float duration, Action<Character> action) : base(duration)
    {
            _action = action;
    }

    public override void OnTick()
    {
        _action?.Invoke(Target);
    }
}

public class OnHitCompositeEffect : DurationEffect, IOnHitEffect
{
    public enum TargetType
    {
        Self,
        Enemy
    }
    
    private Action<Hit> _action;
    private TargetType _targetType;

    public OnHitCompositeEffect(float duration, Action<Hit> action, TargetType targetType) : base(duration)
    {
        _action = action;
        _targetType = targetType;
    }

    public void OnHit(Hit hit)
    {
        if (_targetType == TargetType.Self && hit.Target != Target) return;
        if (_targetType == TargetType.Enemy && hit.Target == Target) return;
        _action?.Invoke(hit);
    }
}