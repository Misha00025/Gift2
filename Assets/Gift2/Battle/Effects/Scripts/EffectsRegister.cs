using System.Collections.Generic;

public class EffectsRegister
{
    public static EffectsRegister Instance { get; protected set; }
    
    private class EffectData
    {
        public IEffect Effect;
        public float AccumulatedTime;
        public float LastTickTime;
        public float Duration;
    }
    
    private List<EffectData> _effects = new List<EffectData>();
    private List<EffectData> _effectsToAdd = new List<EffectData>();
    
    private float _tickRate;
    
    public EffectsRegister(float tickRate)
    {
        if (Instance == null)
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
            effectData.AccumulatedTime += deltaTime;
            effectData.LastTickTime += deltaTime;
            
            if (effectData.Effect is ITikableEffect && (effectData.LastTickTime > _tickRate))
            {
                ((ITikableEffect)effectData.Effect).OnTick();
                effectData.LastTickTime = 0f;
            }
            
            if (effectData.AccumulatedTime >= effectData.Duration)
            {
                _effects[i].Effect.Disable();
                _effects.RemoveAt(i);
            }
        }
    }
    
    public void Register(IEffect effect, float duration = 1f)
    {
        _effectsToAdd.Add(new EffectData
        {
            Effect = effect,
            Duration = duration
        });
    }
    
    public void RemoveEffect(IEffect effect)
    {
        for (int i = _effects.Count - 1; i >= 0; i--)
        {
            if (_effects[i].Effect == effect)
            {
                _effects.RemoveAt(i);
                _effects[i].Effect.Disable();
                break;
            }
        }
        
        for (int i = _effectsToAdd.Count - 1; i >= 0; i--)
        {
            if (_effectsToAdd[i].Effect == effect)
            {
                _effectsToAdd.RemoveAt(i);
                _effects[i].Effect.Disable();
                break;
            }
        }
    }
}