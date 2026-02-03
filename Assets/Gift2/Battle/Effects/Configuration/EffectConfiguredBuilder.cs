using UnityEngine;

// [CreateAssetMenu(fileName = "EffectConfig", menuName = "Effects/EffectConfig")]
public abstract class EffectConfiguredBuilder : ScriptableObject
{
    public Sprite Icon;
    // public EffectView EffectViewPrefab;
    
    public IEffect Build()
    {
        var effect = ConfigureEffect();
        effect.Key = name;
        Debug.Log(effect.Key);
        effect.Icon = Icon;
        return effect;
    }
    
    protected abstract Effect ConfigureEffect();
}

public class EffectConfigsRegister
{
    public static EffectConfigsRegister Instance {get; private set;}
    
    public EffectConfigsRegister()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("EffectConfigsRegister have more that one instance");
    }
}
