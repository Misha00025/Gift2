using UnityEngine;

public class Anivia : Character
{
    [Header("Ice Storm configs")]
    public Damage IceStormDamage = new Damage {Value = 1, Element = Element.Ice};
    public Property Rage = new(){Value = 3, MaxValue = 4};

    public override void Attack()
    {
        if (Rage.Value == Rage.MaxValue)
        {
            IceStorm();
            Rage.Value = 0;
            return;
        }
        base.Attack();
    }

    public override void BaseAttack()
    {
        base.BaseAttack();
        Rage.Value += 1;
    }
    
    private void IceStorm()
    {
        View.AddListener(AnimationEventKey.Hit, IceStormTick);
        View.AddListener(AnimationEventKey.Completed, OnCompleteAttack);
        Animator.Play(AnimationKey.Cast);
    }
    
    private void IceStormTick()
    {
        var hit = PrepareHit(IceStormDamage);
        hit.Apply();
    }
}
