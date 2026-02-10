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
    public Character mainSummon;
    public Character enemy;
    public List<Character> supports = new();
    
    public static BattleMap Map => _instance.map;
    public static BattleLoop Loop => _instance.loop;
    public static Character MainSummon => _instance.mainSummon;
    public static Character Enemy => _instance.enemy;
    public static IReadOnlyList<Character> Supports => _instance.supports;
    
    
    public void Clear()
    {
        if (_instance == this)
            _instance = null;
    }
}