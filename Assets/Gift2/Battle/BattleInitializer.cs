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
        
        if (loop.Enemy.Pivot == null)
            loop.Enemy.Pivot = loop.Enemy.transform;
        
        if (loop.MainSummon.Pivot == null)
            loop.MainSummon.Pivot = loop.MainSummon.transform;
            
        for (var i = 0; i < Supports.Count; i++)
        {
            if (i >= map.SupportSummonPositions.Count) continue;
        
            var point = map.SupportSummonPositions[i];
            var summon = Instantiate(Supports[i], point);
        
            if (summon.Pivot == null)
                summon.Pivot = summon.transform;
            summon.transform.position = map.GetPosition(point, summon);
            summon.SetTarget(loop.Enemy);
            inputHandler.Supports.Add(summon);
        }
            
        loop.MainSummon.transform.position = map.GetPosition(map.MainSummonPosition, loop.MainSummon);
        loop.Enemy.transform.position = map.GetPosition(map.EnemyPosition, loop.Enemy);
    }
}
