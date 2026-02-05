using UnityEngine;

[RequireComponent(typeof(BattleLoop), typeof(BattleMap), typeof(BattleInputHandler))]
public class BattleInitializer : MonoBehaviour
{
    public Character MainSummon;
    public Character Enemy;

    void Awake()
    {
        var loop = GetComponent<BattleLoop>();       
        var map = GetComponent<BattleMap>();       
        var inputHandler = GetComponent<BattleInputHandler>();      
        
        loop.MainSummon = Instantiate(MainSummon, map.Map);
        inputHandler.Character = loop.MainSummon;
        loop.Enemy = Instantiate(Enemy, map.Map);
        
        if (loop.Enemy.Pivot == null)
            loop.Enemy.Pivot = loop.Enemy.transform;
        
        if (loop.MainSummon.Pivot == null)
            loop.MainSummon.Pivot = loop.MainSummon.transform;
            
        loop.MainSummon.transform.position = map.GetPosition(map.MainSummonPosition, loop.MainSummon);
        loop.Enemy.transform.position = map.GetPosition(map.EnemyPosition, loop.Enemy);
    }
}
