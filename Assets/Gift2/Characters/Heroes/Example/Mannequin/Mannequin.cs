using UnityEngine;

public class Mannequin : Character
{
    public float HealDelay = 4.0f;
    private float _remainingHealDelay;
    private int _startHealth;

    public override void Attack(Character target)
    {
        Debug.Log("Mannequin can't attack someone");
        CompleteAttack();
    }

    protected override void Init()
    {
        _remainingHealDelay = HealDelay;
        _startHealth = Health.Value;
        base.Init();
    }
    
    public void Update()
    {
        if (_remainingHealDelay < 0 && Health.Value != _startHealth)
        {
            Health.Value = _startHealth;
        }
        else
        {
            _remainingHealDelay -= Time.deltaTime;
        }
    }

    protected override void SetHealth(int newValue)
    {
        if (newValue <= 0)
            newValue = 1;
        base.SetHealth(newValue);
    }
    

    public override void ApplyDamage(Damage damage)
    {
        _remainingHealDelay = HealDelay;
        base.ApplyDamage(damage);
    }
}
