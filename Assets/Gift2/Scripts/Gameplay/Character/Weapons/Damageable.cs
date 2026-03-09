using System;

public interface IDamageable
{
    void TakeDamage(Damage damage);
}

[Serializable]
public struct Damage
{
    public int Value;
    public float Strength;
    public Element Element;    
}

public enum Element
{
    Physical,
    Axe, Pickaxe
}