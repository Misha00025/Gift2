using System;
using System.Collections.Generic;
using UnityEngine.Events;


[Serializable]
public struct SummonerStats
{
    public float ManaRegeneration;
    public int MaxMana;
    public int MaxSupports;
    
    public static SummonerStats Default => new (){ManaRegeneration = 20f, MaxMana = 100, MaxSupports = 2};
}

public class Summoner
{
    public const int ManaForCast = 40; 

    private SummonerStats _baseStats;
    private SummonerStats _currentStats;
    private List<Character> _supports = new();
    private FloatProperty _mana = new();

    public Character MainCharacter {get; private set;}
    public IReadOnlyList<Character> Supports => _supports;
    public SummonerStats Stats => _currentStats;
    
    public Property Health => MainCharacter.Health;
    public Property Mana => _mana;
    
    public Summoner(Character character) => Initialize(SummonerStats.Default, character);
    public Summoner(SummonerStats stats, Character character) => Initialize(stats, character);
    
    public UnityEvent CastReady {get; private set;} = new();
    
    
    private void Initialize(SummonerStats stats, Character character)
    {
        _baseStats = stats;
        _currentStats = _baseStats;
        MainCharacter = character;
        Mana.MaxValue = Stats.MaxMana;
        Mana.Value = 0;
    }
    
    public void AddSupport(Character character)
    {
        if (_supports.Count >= Stats.MaxSupports) return;
        _supports.Add(character);
    }
    
    public virtual void Tick(float deltaTime)
    {
        var needEvent = !CanCast();
        _mana.AddFloat(Stats.ManaRegeneration * deltaTime);
        if (CanCast() && needEvent)
            CastReady.Invoke();
    }
    
    public bool CanCast() => Mana.Value >= (ManaForCast <= Mana.MaxValue ? ManaForCast : Mana.MaxValue);
    
    public void Cast(Skill skill)
    {
        Mana.Value -= ManaForCast;
        MainCharacter.CancelAttack();
        skill.Play();
    }
}
