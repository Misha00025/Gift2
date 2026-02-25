using UnityEngine;


namespace Gift2.AutoBattler
{
    [CreateAssetMenu(fileName = "TickableEffectConfig", menuName = "Effects/TickableEffectConfig")]
    public class TickableDamageEffectBuilder : EffectConfiguredBuilder
    {
        public float Duration = 1f;
        public Damage DamageOnTick = new(){Value = 1, Element = Element.Fire};

        private class DamageByTickEffect : TikableEffect
        {
            private Damage _damage;
        
            public DamageByTickEffect(Damage damage, float duration) : base(duration)
            {
                _damage = damage;
            }
            public override void OnTick()
            {
                Target.ApplyDamage(_damage);
            }
        }

        protected override Effect ConfigureEffect()
        {
            return new DamageByTickEffect(DamageOnTick, Duration);
        }
    }
}