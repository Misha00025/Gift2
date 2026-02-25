using System.Collections.Generic;
using Gift2.AutoBattler;
using UnityEngine;

public class BattleMap : MonoBehaviour
{
    public static BattleMap Instance {get; private set;}

    [field: SerializeField] public Transform Map {get; private set;}
    
    [field: SerializeField] public Transform CenterPosition {get; private set;}
    
    [field: SerializeField] public Transform EnemyPosition {get; private set;}
    [field: SerializeField] public Transform MainSummonPosition {get; private set;}
    [field: SerializeField] public List<Transform> SupportSummonPositions {get; private set;} = new();
    
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    
    public Vector3 GetPosition(Transform target, Character character)
    {
        var delta = character.transform.position - character.Pivot.transform.position;
        return target.position + delta;
    }
}
