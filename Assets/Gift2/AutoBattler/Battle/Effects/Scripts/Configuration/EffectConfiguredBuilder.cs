using System.Collections.Generic;
using UnityEngine;

namespace Gift2.AutoBattler
{
    // [CreateAssetMenu(fileName = "EffectConfig", menuName = "Effects/EffectConfig")]
    public abstract class EffectConfiguredBuilder : ScriptableObject
    {
        public Sprite Icon;
        public EffectView EffectViewPrefab;
        
        public IEffect Build()
        {
            var effect = ConfigureEffect();
            effect.Key = name;
            effect.Icon = Icon;
            if (EffectViewPrefab != null)
            {
                var register = EffectConfigsRegister.Instance;
                if (register == null)
                    register = new EffectConfigsRegister();
                register.RegisterEffectView(effect.Key, EffectViewPrefab);
            }
            return effect;
        }
        
        protected abstract Effect ConfigureEffect();
    }

    public class EffectConfigsRegister
    {
        public static EffectConfigsRegister Instance {get; private set;}
        
        private Dictionary<string, EffectView> _effectsPrefabs = new();
        private Dictionary<Character, Dictionary<string, EffectView>> _characterViews = new();
        
        public EffectConfigsRegister()
        {
            Instance = this;
        }
        
        
        public void RegisterEffectView(string effectKey, EffectView viewPrefab)
        {
            if (_effectsPrefabs.ContainsKey(effectKey)) return;
        
            _effectsPrefabs.Add(effectKey, viewPrefab);
        }
        
        public EffectView GetView(string effectKey, Character character)
        {
            if (_characterViews.ContainsKey(character) && _characterViews[character].ContainsKey(effectKey))
                return _characterViews[character][effectKey];
        
            if (_effectsPrefabs.ContainsKey(effectKey))
            {
                var view = GameObject.Instantiate(_effectsPrefabs[effectKey], character.transform);
                
                if (!_characterViews.ContainsKey(character))
                    _characterViews.Add(character, new());
                _characterViews[character].Add(effectKey, view);      
                return view;
            }
            
            return null;
        }
    }
}