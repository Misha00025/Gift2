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
    
    public Hit PrepareHit(float multiplayer)
    {
        int damageValue = (int)(Stats.damage * multiplayer);
        var damageElement = Elements.Count > 0 ? Elements[0] : Element.Physical;
        var damage = new Damage(){Value = damageValue, Element = damageElement};
        return base.PrepareHit(damage);
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
