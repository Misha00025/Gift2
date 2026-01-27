using UnityEngine;

public class StoneGolem : Character
{
    public override Damage CalculateDamage(Damage damage)
    {
        if (damage.Element == Element.Stone)
            damage.Value = damage.Value / 2;
        return base.CalculateDamage(damage);
    }
    
}
