using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Damage DefaultDamage = new Damage() {Value = 10, Element = Element.Physical};

    private Character _owner;

    public void SetOwner(Character character)
    {
        _owner = character;
    }
    
    public void OnTriggerEvent(GameObject target)
    {
        var damageable = target.GetComponent<IDamageable>();
        if (damageable == null) return;
        
        var damage = DefaultDamage;
        damage.Value = Mathf.RoundToInt((float)(damage.Value) * _owner.Stats.Strength);
        damageable.TakeDamage(damage);
    }
}
