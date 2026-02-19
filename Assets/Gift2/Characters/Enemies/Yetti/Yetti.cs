using UnityEngine;

public class Yetti : Character
{


    public override void Attack()
    {
        var hit = PrepareHit(new Damage(){Value = Stats.damage, Element = Elements.Count > 0 ? Elements[0] : Element.Physical});
        hit.Apply();
        OnCompleteAttack();
    }
}
