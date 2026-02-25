using System.Collections.Generic;
using Gift2.AutoBattler;
using UnityEngine.Events;

public class Battle
{
    private static Battle _instance;
    
    public enum Condition
    {
        InProcess,
        StepEnded,
        Completed
    }
    
    public Battle()
    {
        if (_instance == null)
            _instance = this;
    }
    
    public BattleMap map;
    public BattleLoop loop;
    public BattleConditionSystem conditionSystem;
    public BattleQueue queue;
    public Character enemy;
    public Summoner summoner;
    
    public static BattleMap Map => _instance?.map;
    public static BattleLoop Loop => _instance?.loop;
    public static Summoner Summoner => _instance?.summoner;
    public static Character Enemy => _instance?.enemy;
    public static Queue<Character> Enemies => _instance?.queue.Enemies;
    public static Character MainSummon => Summoner?.MainCharacter;
    public static IReadOnlyList<Character> Supports => Summoner?.Supports;
    public static Condition CheckCondition() => _instance.conditionSystem.CheckCondition();
    public static UnityEvent StepEnded => _instance?.conditionSystem.StepEnded;
    public static UnityEvent Completed => _instance?.conditionSystem.Completed;
    
    public void Clear()
    {
        if (_instance == this)
            _instance = null;
    }
}