using System;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    
    public Damage AdditionalDamage = new Damage(){Value = 5, Element = Element.Fire};
    public Damage DamageOnTick = new Damage(){Value = 1, Element = Element.Fire};

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

    private class DamageByTickEffect : TikableEffect
    {
        private Damage _damage;
    
        public DamageByTickEffect(Damage damage) : base(4f)
        {
            _damage = damage;
        }
        public override void OnTick()
        {
            Target.ApplyDamage(_damage);
        }
    }
    
    private class EffectByHit : DurationEffect, IOnHitEffect
    {
        private Func<IEffect> _effectFactory;
        public EffectByHit(Func<IEffect> effect) : base(2f)
        {
            _effectFactory = effect;
        }

        public void OnHit(Hit hit)
        {
            hit.Target.ApplyEffect(_effectFactory?.Invoke());
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Character.ApplyEffect(new AdditionalDamageEffect(AdditionalDamage));
            
        if (Input.GetKeyDown(KeyCode.Alpha2))
            for (int i = 0; i < 5; i++)
                Character.ApplyEffect(new AdditionalDamageEffect(AdditionalDamage));
                
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Character.ApplyEffect(new EffectByHit(() => new DamageByTickEffect(DamageOnTick)));  
    }
}
