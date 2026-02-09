using System.Collections.Generic;
using UnityEngine;

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
            EffectConfigsRegister.Instance.RegisterEffectView(effect.Key, EffectViewPrefab);
        return effect;
    }
    
    protected abstract Effect ConfigureEffect();
}

public class EffectConfigsRegister
{
    public static EffectConfigsRegister Instance {get; private set;}
    
    private Dictionary<string, EffectView> _effectsPrefabs = new();
    
    public EffectConfigsRegister()
    {
        Instance = this;
    }
    
    
    public void RegisterEffectView(string effectKey, EffectView viewPrefab)
    {
        if (_effectsPrefabs.ContainsKey(effectKey)) return;
    
        _effectsPrefabs.Add(effectKey, viewPrefab);
    }
    
    public EffectView CreateView(string effectKey, Character character)
    {
        if (_effectsPrefabs.ContainsKey(effectKey))
        {
            var view = GameObject.Instantiate(_effectsPrefabs[effectKey], character.transform);            
            return view;
        }
        
        return null;
    }
}
