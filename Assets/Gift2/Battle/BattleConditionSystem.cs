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
        MainSummon.Health.Changed.AddListener(OnCharacterHealthChange);
    }
    
    private void OnCharacterHealthChange(Property property)
    {
        var condition = CheckCondition();
        if (_lock) return;
        _lock = true;
        switch (condition)
        {
            case Battle.Condition.StepEnded:
                Debug.Log("Step Ended condition");
                StepEnded.Invoke();
                OnEnemyChanged();
                break;
            case Battle.Condition.Completed:
                Completed.Invoke();
                break;
        }
        _lock = false;
    }
    
    public void OnEnemyChanged()
    {
        _enemy?.Health.Changed.RemoveListener(OnCharacterHealthChange);
        _enemy = Enemy;
        _enemy.Health.Changed.AddListener(OnCharacterHealthChange);
    }
    
    public Battle.Condition CheckCondition()
    {
        var playerDie = MainSummon.Health.Value == 0;
        var enemyDie = Enemy.Health.Value == 0;
        
        if (playerDie)
            return Battle.Condition.Completed;
        if (enemyDie && Battle.Enemies.Count > 0)
            return Battle.Condition.StepEnded;
        return Battle.Condition.InProcess;
    }
}