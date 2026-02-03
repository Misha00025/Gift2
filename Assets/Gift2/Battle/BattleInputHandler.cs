using System;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    
    public Damage AdditionalDamage = new Damage(){Value = 5, Element = Element.Fire};
    public TickableDamageEffectBuilder tickableDamageEffectBuilder;


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
            Character.ApplyEffect(new EffectByHit(() => tickableDamageEffectBuilder?.Build()));  
    }
}
