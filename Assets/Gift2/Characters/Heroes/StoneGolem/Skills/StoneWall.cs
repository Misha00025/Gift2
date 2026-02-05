
using System.Collections;
using UnityEngine;


public class StoneWall : Skill<StoneGolem, SkillConfig>
{
    // public Damage Damage = new Damage(){Value = 20, Element = Element.Stone};
    public EffectConfiguredBuilder EffectBuilder;
    
    private Character Target;
    private Vector2 StartPosition;
    
    void Start()
    {
        StartPosition = Caster.transform.position;
    }

    protected override void OnPlay()
    {
        Target = Caster.Target;
        Caster.Animator.Play("JumpUp");
        Caster.Jump.Upped.AddListener(OnUpped);
    }
    
    private void OnUpped()
    {
        Caster.Jump.Upped.RemoveListener(OnUpped);
        Caster.Jump.Grounded.AddListener(OnDowned);
        var map = BattleMap.Instance;
        Caster.transform.position = map.GetPosition(map.CenterPosition, Caster);
        Caster.Animator.Play("JumpDown");
    }
    
    private void OnDowned()
    {
        Caster.Jump.Grounded.RemoveListener(OnDowned);
        Target.ApplyEffect(EffectBuilder.Build());
        StartCoroutine(Delay());        
    }
    
    private IEnumerator Delay(float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        Caster.Jump.Upped.AddListener(OnUppedEnd);
        Caster.Animator.Play("JumpUp");
    }
    
    private void OnUppedEnd()
    {
        Caster.Jump.Upped.RemoveListener(OnUppedEnd);
        Caster.Jump.Downed.AddListener(OnDownedEnd);
        Caster.transform.position = StartPosition;
        Caster.Animator.Play("JumpDown");
    }
    
    private void OnDownedEnd()
    {
        Caster.Jump.Downed.RemoveListener(OnDownedEnd);
        Complete();
    }
}