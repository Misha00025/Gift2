using UnityEngine;

public class FireElemental : Character
{
    public EffectConfiguredBuilder OnAttackEffectBuilder;
    public float ChanceOfFlame = 0.1f;

    public override void Attack()
    {
        Animator.Play("BaseAttack", 1);
    }
    
    public void BaseAttack()
    {
        Target?.ApplyDamage(new Damage(){Value = Stats.damage, Element = Element.Fire});
        if (ChanceOfFlame > Random.Range(0f, 1f))
            Target?.ApplyEffect(OnAttackEffectBuilder.Build());
    }
}
