using System;
using UnityEngine;


namespace Gift2.AutoBattler
{
    [CreateAssetMenu(fileName = "EffectConfig", menuName = "Effects/CompositeEffectConfig")]
    public class CompositeEffectBuilder : EffectConfiguredBuilder
    {
        public EffectLifecycle Lifecycle;
        public EffectBehavior Behavior;
        
        /// <summary>
        /// Определяет жизненный цикл эффекта
        /// </summary>
        public enum EffectLifecycle
        {    
            /// <summary>
            /// Эффект, активируется при каждом ударе
            /// </summary>
            OnHit,
            
            /// <summary>
            /// Эффект, активируется периодически
            /// </summary>
            Tickable,
        }
        
        /// <summary>
        /// Определяет поведение эффекта при активации
        /// </summary>
        public enum EffectBehavior
        {
            /// <summary>
            /// Накладывает другой эффект на цель
            /// </summary>
            ApplyEffect,
            
            /// <summary>
            /// Наносит урон цели
            /// </summary>
            Damage,
        }
        
        public float Duration = 1f;
        
        [Header("OnHit Params")]
        public OnHitCompositeEffect.TargetType TargetType = OnHitCompositeEffect.TargetType.Enemy;
        
        [Header("Apply Effect Params")]
        public EffectConfiguredBuilder ApplyingEffectBuilder;
        
        [Header("Damage Params")]
        public Damage Damage = new(){Value = 1};
        
        protected override Effect ConfigureEffect()
        {
            Effect effect = null;
            switch (Lifecycle)
            {
                case EffectLifecycle.OnHit:
                    effect = CreateOnHitEffect();
                    break;
                case EffectLifecycle.Tickable:
                    effect = CreateTickableEffect();
                    break;
            }
            
            return effect;
        }
        
        
        private Action<Character> CreateAction()
        {
            Action<Character> action = (e) => {};
            
            switch (Behavior)
            {
                case EffectBehavior.ApplyEffect:
                    action = (e) =>
                    {
                        e.ApplyEffect(ApplyingEffectBuilder.Build());  
                    };
                    break;
                case EffectBehavior.Damage:
                    action = (e) => 
                    {
                        e.ApplyDamage(Damage);
                    };
                    break;
            }
            return action;
        }
        
        
        
        private Effect CreateOnHitEffect()
        {
            var action = CreateAction();
            return new OnHitCompositeEffect(Duration, (e) => action(e.Target), TargetType);
        }    
        
        private Effect CreateTickableEffect()
        {
            
            return new TikableCompositeEffect(Duration, CreateAction());
        }
    }
}
