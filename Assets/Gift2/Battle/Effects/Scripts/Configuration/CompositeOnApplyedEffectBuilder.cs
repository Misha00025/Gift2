using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectConfig", menuName = "Effects/CompositeOnAppliedEffectConfig")]
public class CompositeOnAppliedEffectBuilder : EffectConfiguredBuilder
{
    public EffectBehavior Behavior;
    
    public enum EffectBehavior
    {
        Stun
    }
    
    public float Duration = 1f;

    private class OnAppliedCompositeEffect : DurationEffect
    {
        private Action<Character> _onEnable;
        private Action<Character> _onDisable;
    
        public OnAppliedCompositeEffect(float duration, Action<Character> onEnable, Action<Character> onDisable) : base(duration)
        {
            _onEnable = onEnable;
            _onDisable = onDisable;
        }

        public override void Apply(Character character)
        {
            var target = Target;
            base.Apply(character);
            
            if (target == null)
                _onEnable?.Invoke(character);
        }

        public override void Disable()
        {
            var target = Target;
            base.Disable();
            if (target != null)
                _onDisable?.Invoke(target);
        }
    }

    protected override Effect ConfigureEffect()
    {
        switch (Behavior)
        {
            case EffectBehavior.Stun:
                return CreateStun();
        }
        
        return null;
    }
    
    
    private OnAppliedCompositeEffect CreateStun()
    {
        Action<Character> onEnable = (e) =>
        {
            if (e.IsStunned) return;
            e.IsStunned = true;
        };
        Action<Character> onDisable = (e) =>
        {
            e.IsStunned = false;
        };
        var effect = new OnAppliedCompositeEffect(Duration, onEnable, onDisable);
        return effect;        
    }
}