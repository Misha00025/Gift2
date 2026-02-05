using UnityEngine.Events;

public class StoneGolem : Character
{
    public class JumpEvents
    {
        public UnityEvent Upped = new();
        public UnityEvent Grounded = new();
        public UnityEvent Downed = new();
        public enum State
        {
            Upped,
            Grounded,
            Downed
        }
    }

    public JumpEvents Jump { get; private set; } = new();

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
        else if (state == JumpEvents.State.Grounded)
            Jump.Grounded.Invoke();
        else
            Jump.Downed.Invoke();
    }
}
