using UnityEngine;

public class Mannequin : Character
{
    public int HealDelayByTicks;
    
    private HealToFull _onTargetEffect;

    private class HealToFull : Effect, ITikableEffect
    { 
        private int _ticksToHeal;
        private int _ticks = 0;

        public HealToFull(int ticks = 4){_ticksToHeal = ticks;}

        public override void Apply(Character character)
        {
            character.DamageTaken.AddListener((e) => {_ticks = 0;});
            base.Apply(character);
            EffectsRegister.Instance.Register(this, -1);
        }
        

        public void OnTick()
        {
            _ticks++;
            if (_ticks == _ticksToHeal)
            {
                _ticks = 0;       
                Target.Health.Value = Target.Health.MaxValue;
            }
        }
    }

    public override void Attack()
    {
        if (Target != null && _onTargetEffect == null)
        {
            var effect = new HealToFull();
            effect.Apply(Target);
            _onTargetEffect = effect;
        }
    
        BaseAttack();
        CompleteAttack();
    }

    protected override void Init()
    {
        var effect = new HealToFull();
        effect.Apply(this);
        base.Init();
    }


    protected override void SetHealth(int newValue)
    {
        if (newValue <= 0)
            newValue = 1;
        base.SetHealth(newValue);
    }
}
