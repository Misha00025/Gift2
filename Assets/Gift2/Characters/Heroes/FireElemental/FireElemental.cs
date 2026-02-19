using UnityEngine;

[RequireComponent(typeof(FireElementalCastHandler))]
public class FireElemental : Character
{
    public EffectConfiguredBuilder OnAttackEffectBuilder;
    public float ChanceOfFlame = 0.1f;
    public FireElementalCastHandler CastHandler {get; private set;}

    protected override void Init()
    {
        CastHandler = GetComponent<FireElementalCastHandler>();
        base.Init();
    }
    
    public override void BaseAttack()
    {
        base.BaseAttack();
        if (ChanceOfFlame > Random.Range(0f, 1f))
            Target?.ApplyEffect(OnAttackEffectBuilder.Build());
    }
}
