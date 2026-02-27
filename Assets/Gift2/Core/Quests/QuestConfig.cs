using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gift2.Core
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/QuestConfig")]
    public class QuestConfig : ScriptableObject
    {
        public string questName;
        [TextArea] public string description;
        public List<QuestGoalConfig> goals;
        public List<QuestRewardConfig> rewards;
        public bool isRepeatable;

        /// <summary>
        /// Создаёт экземпляр квеста с целями и наградами, описанными в конфиге.
        /// </summary>
        public Quest Build()
        {
            // Создаём квест с именем и описанием
            var quest = new Quest(questName, description);

            // Добавляем цели (они сами подпишутся на квест через конструктор)
            foreach (var goalConfig in goals)
            {
                goalConfig.CreateGoal(quest);
            }

            // Добавляем награды
            foreach (var rewardConfig in rewards)
            {
                rewardConfig.CreateReward(quest);
            }

            return quest;
        }
    }
    
    [Serializable]
    public class QuestRewardConfig
    {
        public RewardType type;
        
        public Quest.QuestReward CreateReward(Quest quest)
        {
            switch (type)
            {
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [Serializable]
    public class QuestGoalConfig
    {
        public GoalType type;

        public Quest.QuestGoal CreateGoal(Quest quest)
        {
            switch (type)
            {
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum GoalType
    {
        Delivery
    }

    public enum RewardType
    {
        Item
    }
}