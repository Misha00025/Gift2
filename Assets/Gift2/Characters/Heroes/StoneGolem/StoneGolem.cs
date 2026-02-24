using UnityEngine.Events;

public class StoneGolem : Character
{
    public Hit PrepareHit(float multiplayer)
    {
        int damageValue = (int)(Stats.damage * multiplayer);
        var damageElement = Elements.Count > 0 ? Elements[0] : Element.Physical;
        var damage = new Damage(){Value = damageValue, Element = damageElement};
        return base.PrepareHit(damage);
    }
}
