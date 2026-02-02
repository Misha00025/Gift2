using System;

public class Hit
{
    public Character Target;
    public event Action<Hit> Applied;
    public Damage Damage;
    
    public void Apply()
    {
        Target?.ApplyDamage(Damage);
        Applied?.Invoke(this);
    }
}