using System;
using UnityEngine;


namespace Gift2.AutoBattler
{
    [CreateAssetMenu(fileName = "EffectConfig", menuName = "Effects/CompositeOnAppliedEffectConfig")]
    public class CompositeOnAppliedEffectBuilder : EffectConfiguredBuilder
    {
        public EffectBehavior Behavior;
        
        public enum EffectBehavior
        {
            Stun
        }
        
        public float Duration = 1f;

        private class OnAppliedCompositeEffect : DurationEffect
        {
            private Action<Character> _onEnable;
            private Action<Character> _onDisable;
            
            private bool _active = false;
        
            public OnAppliedCompositeEffect(float duration, Action<Character> onEnable, Action<Character> onDisable) : base(duration)
            {
                _onEnable = onEnable;
                _onDisable = onDisable;
            }

            public override void Apply(Character character)
            {
                var target = Target;
                base.Apply(character);
                
                if (target == null && _active == false)
                    _onEnable?.Invoke(character);
                _active = true;
            }

            public override void Disable()
            {
                var target = Target;
                base.Disable();
                if (target != null && _active)
                    _onDisable?.Invoke(target);
                _active = false;
            }
        }

        protected override Effect ConfigureEffect()
        {
            switch (Behavior)
            {
                case EffectBehavior.Stun:
                    return CreateStun();
            }
            
            return null;
        }
        
        
        private OnAppliedCompositeEffect CreateStun()
        {
            Action<Character> onEnable = (e) =>
            {
                e.AddStun();
            };
            Action<Character> onDisable = (e) =>
            {
                e.RemoveStun();
            };
            var effect = new OnAppliedCompositeEffect(Duration, onEnable, onDisable);
            return effect;        
        }
    }
}