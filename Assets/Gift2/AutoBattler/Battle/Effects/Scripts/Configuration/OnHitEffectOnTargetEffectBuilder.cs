using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHitEffectOnTargetEffectBuilder", menuName = "Effects/OnHitEffectOnTargetEffectBuilder")]
public class OnHitEffectOnTargetEffectBuilder : EffectConfiguredBuilder
{
    public float Duration = 1f;
    public EffectConfiguredBuilder effectConfiguredBuilder;

    private class EffectByHit : DurationEffect, IOnHitEffect
    {
        private Func<IEffect> _effectFactory;
        public EffectByHit(float duration, Func<IEffect> effect) : base(duration)
        {
            _effectFactory = effect;
        }

        public void OnHit(Hit hit)
        {   
            if (hit.Target != Target)
                hit.Target.ApplyEffect(_effectFactory?.Invoke());
        }
        
    }

    protected override Effect ConfigureEffect()
    {
        return new EffectByHit(Duration, () => effectConfiguredBuilder.Build());
    }
}
