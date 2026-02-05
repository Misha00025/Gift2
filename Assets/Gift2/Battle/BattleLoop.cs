using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public Character MainSummon;
    public Character Enemy;
    public float TickRate = 0.5f;
    public float FastSpeed = 3f;

    private List<Character> Characters = new();
    private Dictionary<Character, float> RemainingTimes = new();
    
    private Character _currentAttacker;
    private EffectsRegister _effectsRegister;
    private EffectConfigsRegister _effectsConfigRegister;
    
    public bool PauseOnStart = false;
    private bool _paused;
    private bool _attackInProgress;

    public bool Paused => _paused;

    void Awake()
    {
        _effectsRegister = new(TickRate);
        _effectsConfigRegister = new EffectConfigsRegister();
    }

    void Start()
    {    
        _attackInProgress = PauseOnStart;
        Characters = new(){ MainSummon, Enemy };
        foreach (var character in Characters)
        {
            character.AttackCompleted.AddListener(OnCharacterAttackCompleted);
            character.SetTarget(GetTarget(character));
            RemainingTimes.Add(character, GetRemaining(character));
        }
    }

    void Update()
    {
        if (_paused) return;
        
        _effectsRegister.Update(Time.deltaTime);
        
        if (_attackInProgress) return;
        
        foreach (var character in Characters)
        {
            var remainingTime = RemainingTimes[character];
            if (remainingTime < 0)
            {
                _attackInProgress = true;
                _currentAttacker = character;
                RemainingTimes[character] = GetRemaining(character);
                character.Attack();
                return;
            }
            RemainingTimes[character] -= Time.deltaTime;
        }
    }
    
    public void SetPause(bool pause = true)
    {
        _paused = pause;
        _attackInProgress = false;
    }
    
    public void ToggleSpeed()
    {
        if (Time.timeScale > 1.1f)
            SetNormalSpeed();
        else
            SetFastSpeed();
    }
    
    public void SetNormalSpeed()
    {
        Time.timeScale = 1f;
    }
    
    public void SetFastSpeed()
    {
        Time.timeScale = FastSpeed;
    }
    
    void FixedUpdate()
    {
        if (_attackInProgress) return;
        
    }
    
    private float GetRemaining(Character character)
    {
        return 1f / character.Stats.attackSpeed;
    }
    
    private Character GetTarget(Character attacker)
    {
        if (attacker == MainSummon)
            return Enemy;
        else
            return MainSummon;
    }
    
    private void OnCharacterAttackCompleted(Character character)
    {
        var validCharacter = _currentAttacker == character;
        if (validCharacter)
        {
            _currentAttacker = null;
            _attackInProgress = false;
        }
    }
}
