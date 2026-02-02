public class StoneGolem : Character
{
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
    
}
