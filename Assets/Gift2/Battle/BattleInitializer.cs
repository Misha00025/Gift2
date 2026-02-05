using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BattleLoop), typeof(BattleMap), typeof(BattleInputHandler))]
public class BattleInitializer : MonoBehaviour
{
    public Character MainSummon;
    public Character Enemy;
    public List<Character> Supports = new();

    void Awake()
    {
        var loop = GetComponent<BattleLoop>();       
        var map = GetComponent<BattleMap>();       
        var inputHandler = GetComponent<BattleInputHandler>();      
        
        loop.MainSummon = Instantiate(MainSummon, map.MainSummonPosition);
        inputHandler.Character = loop.MainSummon;
        loop.Enemy = Instantiate(Enemy, map.EnemyPosition);
        
        for (var i = 0; i < Supports.Count; i++)
        {
            if (i >= map.SupportSummonPositions.Count) continue;
        
            var point = map.SupportSummonPositions[i];
            var summon = Instantiate(Supports[i], point);
        
            summon.View.SetOn(point.position);
            summon.SetTarget(loop.MainSummon);
            inputHandler.Supports.Add(summon);
        }
            
        loop.MainSummon.View.SetOn(map.MainSummonPosition.position);
        loop.Enemy.View.SetOn(map.EnemyPosition.position);
    }
}
