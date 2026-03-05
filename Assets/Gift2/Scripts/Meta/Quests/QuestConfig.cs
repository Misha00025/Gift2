using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gift2.Core
{
    public abstract class QuestConfig : ScriptableObject
    {
        public string Name;
        [TextArea] public string Description;

        public abstract Quest Build();
    }
    
    public abstract class QuestConfig<TGoal, TReward> : QuestConfig where TGoal : QuestGoalConfig where TReward : QuestRewardConfig
    {
        public List<TGoal> goals;
        public List<TReward> rewards;

        public override Quest Build()
        {
            var quest = new Quest(Name, Description);

            foreach (var goalConfig in goals)
            {
                goalConfig.CreateGoal(quest);
            }

            foreach (var rewardConfig in rewards)
            {
                rewardConfig.CreateReward(quest);
            }

            return quest;
        }
    }
    
    public abstract class QuestRewardConfig
    {
        public abstract Quest.QuestReward CreateReward(Quest quest);
    }

    public abstract class QuestGoalConfig
    {
        public abstract Quest.QuestGoal CreateGoal(Quest quest);
    }
    
    public abstract class QuestRewardConfig<T> : QuestRewardConfig where T : Quest.QuestReward
    {
    }
    
    public abstract class QuestGoalConfig<T> : QuestGoalConfig where T : Quest.QuestGoal
    {
    }
}