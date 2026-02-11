using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public Character MainSummon;
    public Character Enemy;
    public Summoner Summoner;
    public TimeScaler TimeScaler;
    public float TickRate = 0.5f;

    private List<Character> Characters = new();
    private Dictionary<Character, float> RemainingTimes = new();
    
    private Character _currentAttacker;
    private EffectsRegister _effectsRegister => EffectsRegister.Instance;
    
    [SerializeField] private bool _paused;
    private bool _attackInProgress;

    private float _accumulatedTime = 0f;

    private bool CanAttack => !(_attackInProgress || _paused || _currentAttacker != null);

    public bool Paused => _paused;

    public void Initialize(Character enemy, Summoner summoner)
    {
        MainSummon = summoner.MainCharacter;
        Enemy = enemy;
        Summoner = summoner;
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
        _accumulatedTime += Time.deltaTime;
        if (_accumulatedTime > TickRate)
        {
            Tick();
            _accumulatedTime = 0;
        }
        
        _effectsRegister.Update(Time.deltaTime);
        
        ProcessAttacks();
    }
    
    private void ProcessAttacks()
    {
        foreach (var character in Characters)
        {
            var remainingTime = RemainingTimes[character];
            if (remainingTime < 0 && CanAttack)
            {
                _attackInProgress = true;
                _currentAttacker = character;
                character.Attack();
                return;
            }
            RemainingTimes[character] -= Time.deltaTime;
        }
    }
    
    private void Tick()
    {
        Summoner.Tick(TickRate);
        _effectsRegister.Tick();
    }
    
    public void SetPause(bool pause = true)
    {
        _paused = pause;
        _attackInProgress = false;
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
        if (RemainingTimes.ContainsKey(character))
        RemainingTimes[character] = GetRemaining(character);
        var validCharacter = _currentAttacker == character;
        if (validCharacter)
        {
            _currentAttacker = null;
            _attackInProgress = false;
        }
    }
}
