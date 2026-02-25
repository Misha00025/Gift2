using System;
using Gift2.AutoBattler;

public class Hit
{
    public Character Target;
    public event Action<Hit> Applied;
    public event Action<Hit> Canceled;
    public Damage Damage;
    public bool IsCanceled { get; private set; }
    
    public void Cancel()
    {
        IsCanceled = true;
        Canceled?.Invoke(this);
    }
    
    public void Apply()
    {
        if (IsCanceled) return;
        
        Target?.ApplyDamage(Damage);
        Applied?.Invoke(this);
    }
}