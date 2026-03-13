using System;
using Gift2.Core;
using Gift2.Meta;
using UnityEngine;

public class QuestDialer : MonoBehaviour
{
    [field: SerializeField] public string Key { get; private set; }
    
    public QuestConfig QuestConfig;
    public bool Completed { get; private set; }
    
    void Awake()
    {
        if (String.IsNullOrEmpty(Key))
            Key = name;
    }
    
    public Quest AcceptQuest(Player player)
    {
        if (QuestConfig == null || Completed) return null;
        
        var quest = QuestConfig.Build();
        quest.Completed.AddListener(OnComplete);
        quest.Accept(player);
        QuestsManager.Instance.AddQuest(quest, this);
        return quest;
    }
    
    public bool CompleteQuest(Quest quest)
    {
        if (QuestsManager.Instance.GetDealerKey(quest) != Key) return false;
        
        var status = quest.GoalsIsReached() == true;
        if (status)
        {
            quest.Complete();
        }
        return status;
    }
    
    private void OnComplete(Quest quest)
    {
        if (QuestConfig.Repeatable == false)
            Completed = true;
    }
}
