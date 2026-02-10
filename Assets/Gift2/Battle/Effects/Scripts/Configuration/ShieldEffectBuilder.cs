using UnityEngine;

[CreateAssetMenu(fileName = "ShieldConfig", menuName = "Effects/ShieldConfig")]
public class ShieldEffectsBuilder : EffectConfiguredBuilder
{
    public float Duration = 0.5f;

    private class ShieldEffect : DurationEffect, IOnHitAttemptEffect
    {
        private EffectView _view;
    
        public ShieldEffect(float duration) : base(duration)
        {
        }

        public override void Apply(Character character)
        {
            base.Apply(character);   
            _view = EffectConfigsRegister.Instance.CreateView(Key, character);
        }

        public void OnHitAttempt(Hit hit)
        {
            if (hit.Target != Target) return;
            hit.Cancel();
            _view?.Animator?.Play("Block");
        }
    }
    
    protected override Effect ConfigureEffect()
    {
        return new ShieldEffect(Duration);
    }
}
