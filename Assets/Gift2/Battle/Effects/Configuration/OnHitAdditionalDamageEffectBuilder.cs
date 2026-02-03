using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHitAdditionalDamageEffectBuilder", menuName = "Effects/OnHitAdditionalDamageEffectBuilder")]
public class OnHitAdditionalDamageEffectBuilder : EffectConfiguredBuilder
{
    public Damage AdditionalDamage = new(){Value = 1, Element = Element.Physical};

    private class AdditionalDamageEffect : OneHitEffect
    {
        private Damage _damage;
        public AdditionalDamageEffect(Damage damage)
        {
            _damage = damage;
        }

        public override void OnHit(Hit hit)
        {
            hit.Target.ApplyDamage(_damage);
            base.OnHit(hit);
        }
    }

    protected override Effect ConfigureEffect()
    {
        return new AdditionalDamageEffect(AdditionalDamage);
    }
}
