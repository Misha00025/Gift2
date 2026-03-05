using System;
using Gift2.Core;
using UnityEngine;

namespace Gift2.Meta
{
    [CreateAssetMenu(fileName = "DeliverQuest", menuName = "Quests/DeliverQuest")]
    public class DeliverQuest : QuestConfig<DeliverGoalConfig, ItemRewardConfig>
    {
    
    }

    [Serializable]
    public class DeliverGoalConfig : QuestGoalConfig
    {
        public ItemConfig Item;
        public int Amount = 1;
        
        public override Quest.QuestGoal CreateGoal(Quest quest)
        {
            var data = new DeliverGoal.DeliverData(Item.Build(), Amount);
            var goal = new DeliverGoal(quest, data);
            return goal;
        }
    }

    [Serializable]
    public class ItemRewardConfig : QuestRewardConfig
    {
        public ItemConfig Item;
        public int Amount = 1;
    
        public override Quest.QuestReward CreateReward(Quest quest)
        {
            return new ItemQuestReward(quest, Item.Build(), Amount);
        }
    }
}
