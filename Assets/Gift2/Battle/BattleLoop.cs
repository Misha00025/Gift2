using System.Collections.Generic;
using UnityEngine;

public class BattleLoop : MonoBehaviour
{
    public Character Player;
    public Character Enemy;

    private List<Character> Characters = new();
    private Dictionary<Character, float> RemainingTimes = new();
    
    private Character _currentAttacker;
    
    public bool PauseOnStart = false;
    private bool _paused;

    void Start()
    {
        _paused = PauseOnStart;
        Characters = new(){ Player, Enemy };
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
        foreach (var character in Characters)
        {
            var remainingTime = RemainingTimes[character];
            if (remainingTime < 0)
            {
                _paused = true;
                _currentAttacker = character;
                RemainingTimes[character] = GetRemaining(character);
                character.Attack();
                return;
            }
            RemainingTimes[character] -= Time.deltaTime;
        }
    }
    
    private float GetRemaining(Character character)
    {
        return 1f / character.Stats.attackSpeed;
    }
    
    private Character GetTarget(Character attacker)
    {
        if (attacker == Player)
            return Enemy;
        else
            return Player;
    }
    
    private void OnCharacterAttackCompleted(Character character)
    {
        var validCharacter = _currentAttacker == character;
        if (validCharacter)
        {
            _currentAttacker = null;
            _paused = false;
        }
    }
}
