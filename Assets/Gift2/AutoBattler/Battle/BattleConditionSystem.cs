using UnityEngine;
using UnityEngine.Events;

public class BattleConditionSystem
{
    private Character MainSummon => Battle.MainSummon;
    private Character Enemy => Battle.Enemy;
    private Character _enemy;
    
    private Battle _battle;
    private bool _lock = false;
    
    public UnityEvent StepEnded {get; private set;} = new();
    public UnityEvent Completed {get; private set;} = new();

    public BattleConditionSystem(Battle battle)
    {
        _battle = battle;
    }
    
    public void ProcessCondition()
    {
        var condition = CheckCondition();
        if (_lock) return;
        _lock = true;
        switch (condition)
        {
            case Battle.Condition.StepEnded:
                Debug.Log("Step Ended condition");
                StepEnded.Invoke();
                break;
            case Battle.Condition.Completed:
                Completed.Invoke();
                break;
        }
        _lock = false;
    }
    
    public Battle.Condition CheckCondition()
    {
        var playerDie = MainSummon.Health.Value == 0;
        var enemyDie = Enemy?.Health.Value == 0;
        
        if (playerDie)
            return Battle.Condition.Completed;
        if (enemyDie && Battle.Enemies.Count > 0)
            return Battle.Condition.StepEnded;
        else if (enemyDie)
            return Battle.Condition.Completed;
        return Battle.Condition.InProcess;
    }
}