
using UnityEngine;


public class StoneWall : Skill<StoneGolem, SkillConfig>
{
    public Damage Damage = new Damage(){Value = 20, Element = Element.Stone};
    
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
        Caster.Jump.Downed.AddListener(OnDowned);
        Caster.transform.position = Target.transform.position;
        Caster.Animator.Play("JumpDown");
    }
    
    private void OnDowned()
    {
        Caster.Jump.Downed.RemoveListener(OnDowned);
        Caster.Jump.Upped.AddListener(OnUppedEnd);
        Target.ApplyDamage(Damage);
        Caster.Animator.Play("JumpUp");        
    }
    
    private void OnUppedEnd()
    {
        Caster.Jump.Upped.RemoveListener(OnDownedEnd);
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