public class StoneGolem : Character
{
    private Character _target;

    public override void Attack(Character target)
    {
        _target = target;
        Animator?.Play("BaseAttack");
    }
    
    public void CompleteAttack()
    {
        if (_target == null) return;
        
        var damage = new Damage(){ Value = 5, Element = Element.Stone};
        damage = _target.CalculateDamage(damage);
        _target.ApplyDamage(damage);
    }

    public override Damage CalculateDamage(Damage damage)
    {
        if (damage.Element == Element.Stone)
            damage.Value = damage.Value / 2;
        return base.CalculateDamage(damage);
    }
    
}
