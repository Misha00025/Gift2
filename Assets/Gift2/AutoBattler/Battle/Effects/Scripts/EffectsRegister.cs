using System.Collections.Generic;

public class EffectsRegister
{
    public static EffectsRegister Instance { get; protected set; }
    
    private class EffectData
    {
        public IEffect Effect;
        public float AccumulatedTime;
        public float Duration;
        public bool IsRemoved;
    }
    
    private List<EffectData> _effects = new List<EffectData>();
    private List<EffectData> _effectsToAdd = new List<EffectData>();
    private Dictionary<IEffect, EffectData> _effectsData = new();
    
    private float _tickRate;
    
    public EffectsRegister(float tickRate)
    {
        Instance = this;
        _tickRate = tickRate;
    }
    
    public void Update(float deltaTime)
    {
        if (_effectsToAdd.Count > 0)
        {
            _effects.AddRange(_effectsToAdd);
            _effectsToAdd.Clear();
        }
        
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            var effectData = _effects[i];
            if (effectData.IsRemoved) {
                _effects.Remove(effectData);
                continue;
            }
            if (effectData.Duration > 0f)
                effectData.AccumulatedTime += deltaTime;
            
            if (effectData.Duration > 0f && effectData.AccumulatedTime >= effectData.Duration)
            {
                _effects[i].Effect.Disable();
                _effects.RemoveAt(i);
            }
        }
    }
    
    public void Tick()
    {
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            var effectData = _effects[i];
            if (effectData.IsRemoved) {
                _effects.Remove(effectData);
                continue;
            }
            if (effectData.Effect is ITikableEffect)
            {
                ((ITikableEffect)effectData.Effect).OnTick();
            }
        }
    
    }
    
    public void Register(IEffect effect, float duration = 1f)
    {
        var data = new EffectData
        {
            Effect = effect,
            Duration = duration
        };
        _effectsToAdd.Add(data);
        if (_effectsData.ContainsKey(effect) == false)
            _effectsData.Add(effect, data);
    }
    
    public void RemoveEffect(IEffect effect)
    {
        if (_effectsData.ContainsKey(effect) == false) return;
        
        var data = _effectsData[effect];
        data.IsRemoved = true;
        _effectsData.Remove(effect);
    }
    
    public void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}