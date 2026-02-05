using UnityEngine;

[CreateAssetMenu(fileName = "ShieldConfig", menuName = "Effects/ShieldConfig")]
public class ShieldEffectsBuilder : EffectConfiguredBuilder
{
    public float Duration = 0.5f;

    private class ShieldEffect : DurationEffect, IBeforeTakeDamageEffect
    {
        public ShieldEffect(float duration) : base(duration)
        {
        }

        public void OnDamageTaking(ref Damage damage)
        {
            damage.Value = 0;
        }
    }
    
    protected override Effect ConfigureEffect()
    {
        return new ShieldEffect(Duration);
    }
}
