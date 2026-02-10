using UnityEngine;

public class HookFromLeft : Skill<StoneGolem, SkillConfig>
{
    public float DamageMultiplayer = 2f;

    protected override void OnPlay()
    {
        Caster.Animator.Play("HookFromLeft");
    }
    
    public void OnHook()
    {
        var hit = Caster.PrepareHit(DamageMultiplayer);
        hit.Apply();
    }
    
    public void OnHookEnd()
    {
        Complete();
    }
}
