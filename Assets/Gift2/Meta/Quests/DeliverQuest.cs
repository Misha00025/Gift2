using Gift2.Core;
using UnityEngine;

namespace Gift2.Meta
{
    [CreateAssetMenu(fileName = "DeliverQuest", menuName = "Quests/DeliverQuest")]
    public class DeliverQuest : QuestConfig<DeliverGoalConfig, ItemRewardConfig>
    {
    
    }

    public class DeliverGoalConfig : QuestGoalConfig
    {
        public override Quest.QuestGoal CreateGoal(Quest quest)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ItemRewardConfig : QuestRewardConfig
    {
        public override Quest.QuestReward CreateReward(Quest quest)
        {
            throw new System.NotImplementedException();
        }
    }
}
