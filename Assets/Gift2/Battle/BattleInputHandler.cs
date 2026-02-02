using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    public Character Character;
    
    public Damage AdditionalDamage = new Damage(){Value = 5, Element = Element.Fire};

    private class DamageEffect : OneHitEffect
    {
        private Damage _damage;
        public DamageEffect(Damage damage)
        {
            _damage = damage;
        }

        public override void OnHit(Hit hit)
        {
            hit.Target.ApplyDamage(_damage);
            base.OnHit(hit);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Character.ApplyEffect(new DamageEffect(AdditionalDamage));
    }
}
