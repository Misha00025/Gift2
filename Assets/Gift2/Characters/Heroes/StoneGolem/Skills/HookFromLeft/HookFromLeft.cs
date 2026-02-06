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
        int damageValue = (int)(Caster.Stats.damage * DamageMultiplayer);
        var damageElement = Caster.Elements.Count > 0 ? Caster.Elements[0] : Element.Physical;
        var damage = new Damage(){Value = damageValue, Element = damageElement};
        Caster.Target.ApplyDamage(damage);
    }
    
    public void OnHookEnd()
    {
        Complete();
    }
}
