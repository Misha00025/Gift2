using Gift2.AutoBattler;
using UnityEngine;

public class FireAura : Skill<FireElemental, SkillConfig>
{
    public EffectConfiguredBuilder EffectBuilder;

    protected override void OnPlay()
    {
        _caster.CastHandler.Casted.AddListener(OnCastFireAura);
        _caster.CastHandler.Ended.AddListener(OnCastEnd);
        _caster.Animator.Play("Cast");
    }
    
    private void OnCastFireAura()
    {
        _caster.CastHandler.Casted.RemoveListener(OnCastFireAura);
        _caster.Target.ApplyEffect(EffectBuilder.Build());
    }
    
    private void OnCastEnd()
    {
        _caster.CastHandler.Ended.RemoveListener(OnCastEnd);
        Complete();
    }
}
