using System;

public interface IDamageable
{
    void TakeDamage(Damage damage);
}

[Serializable]
public struct Damage
{
    public int Value;
    public Element Element;    
}

public enum Element
{
    Physical,
    Fire, Ice, Water, 
    Stone, Wind, Plant,
}