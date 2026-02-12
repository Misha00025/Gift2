using UnityEngine;

public class HookFromLeft : Skill<StoneGolem, SkillConfig>
{
    public float DamageMultiplayer = 2f;
    public EffectConfiguredBuilder EffectOnHit;

    protected override void OnPlay()
    {
        Caster.Animator.Play("HookFromLeft");
    }
    
    public void OnHook()
    {
        var hit = Caster.PrepareHit(DamageMultiplayer);
        var effect = EffectOnHit.Build();
        hit.Apply();
        if (hit.IsCanceled == false)
            effect.Apply(hit.Target);
    }
    
    public void OnHookEnd()
    {
        Complete();
    }
}
