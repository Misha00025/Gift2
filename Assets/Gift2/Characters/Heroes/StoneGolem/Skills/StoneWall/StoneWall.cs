
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
        StartPosition = _caster.transform.position;
    }

    protected override void OnPlay()
    {
        Target = _caster.Target;
        _caster.Animator.Play("JumpUp");
        _caster.View.AddListener("JumpUp", OnUpped);
    }
    
    private void OnUpped()
    {
        _caster.View.RemoveListener("JumpUp", OnUpped);
        _caster.View.AddListener("Grounded", OnDowned);
        var map = BattleMap.Instance;
        _caster.View.SetOn(map.CenterPosition.position);
        _caster.Animator.Play("JumpDown");
    }
    
    private void OnDowned()
    {
        _caster.View.RemoveListener("Grounded", OnDowned);
        Target.ApplyEffect(EffectBuilder.Build());
        StartCoroutine(Delay());        
    }
    
    private IEnumerator Delay(float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);
        _caster.View.AddListener("JumpUp",OnUppedEnd);
        _caster.Animator.Play("JumpUp");
    }
    
    private void OnUppedEnd()
    {
        _caster.View.RemoveListener("JumpUp", OnUppedEnd);
        _caster.View.AddListener("Completed", OnDownedEnd);
        _caster.transform.position = StartPosition;
        _caster.Animator.Play("JumpDown");
    }
    
    private void OnDownedEnd()
    {
        _caster.View.RemoveListener("Completed", OnDownedEnd);
        Complete();
    }
}