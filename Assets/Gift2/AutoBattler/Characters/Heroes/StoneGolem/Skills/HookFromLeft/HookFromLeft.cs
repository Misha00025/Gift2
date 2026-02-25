using UnityEngine;

public class HookFromLeft : Skill<StoneGolem, SkillConfig>
{
    public float DamageMultiplayer = 2f;
    public EffectConfiguredBuilder EffectOnHit;

    protected override void OnPlay()
    {
        _caster.Animator.Play("HookFromLeft");
    }
    
    public void OnHook()
    {
        var hit = _caster.PrepareHit(DamageMultiplayer);
        var effect = EffectOnHit.Build();
        hit.Apply();
        if (hit.IsCanceled == false)
            hit.Target.ApplyEffect(effect);
    }
    
    public void OnHookEnd()
    {
        Complete();
    }
}
