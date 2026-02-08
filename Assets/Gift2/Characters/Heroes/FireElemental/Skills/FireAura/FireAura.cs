using UnityEngine;

public class FireAura : Skill<FireElemental, SkillConfig>
{
    public EffectConfiguredBuilder EffectBuilder;

    protected override void OnPlay()
    {
        Caster.CastHandler.Casted.AddListener(OnCastFireAura);
        Caster.CastHandler.Ended.AddListener(OnCastEnd);
        Caster.Animator.Play("Cast");
    }
    
    private void OnCastFireAura()
    {
        Caster.CastHandler.Casted.RemoveListener(OnCastFireAura);
        Caster.Target.ApplyEffect(EffectBuilder.Build());
    }
    
    private void OnCastEnd()
    {
        Caster.CastHandler.Ended.RemoveListener(OnCastEnd);
        Complete();
    }
}
