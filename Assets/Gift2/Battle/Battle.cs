using System.Collections.Generic;

public class Battle
{
    private static Battle _instance;
    
    public Battle()
    {
        if (_instance == null)
            _instance = this;
    }
    
    public BattleMap map;
    public BattleLoop loop;
    public Character enemy;
    public Summoner summoner;
    
    public static BattleMap Map => _instance.map;
    public static BattleLoop Loop => _instance.loop;
    public static Summoner Summoner => _instance.summoner;
    public static Character Enemy => _instance.enemy;
    public static Character MainSummon => Summoner.MainCharacter;
    public static IReadOnlyList<Character> Supports => Summoner.Supports;
    
    
    public void Clear()
    {
        if (_instance == this)
            _instance = null;
    }
}