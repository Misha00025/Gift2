using UnityEngine.Events;

public class StoneGolem : Character
{
    public struct JumpEvents
    {
        public UnityEvent Upped;
        public UnityEvent Downed;
        public enum State
        {
            Upped,
            Downed
        }
    }

    public JumpEvents Jump { get; private set; } = new(){Upped = new(), Downed = new()};

    public override void Attack()
    {
        Animator?.Play("BaseAttack");
    }
    
    public void BaseAttack()
    {
        if (Target == null) return;
        
        var hit = PrepareHit(new Damage(){ Value = Stats.damage, Element = Element.Stone});
        hit.Apply();
    }
    
    public void OnJump(JumpEvents.State state)
    {
        if (state == JumpEvents.State.Upped)
            Jump.Upped.Invoke();
        else
            Jump.Downed.Invoke();
    }
}
