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
        public enum ItemRewardType
        {
            Item,
            Weapon
        }

        public ItemRewardType RewardType;
        public Weapon WeaponOnAdd;
        public ItemConfig Item;
        public int Amount = 1;

        private class WeaponQuestReward : Quest.QuestReward
        {
            private Weapon _prefab;
            private int _amount;
            public WeaponQuestReward(Quest quest, int amount, Weapon weapon) : base(quest)
            {
                _prefab = weapon;
                _amount = amount;
            }

            public override void Give()
            {
                for (int i = 0; i < _amount; i++)
                    Quest.Player.Character.AddWeapon(_prefab);
            }
        }

        public override Quest.QuestReward CreateReward(Quest quest)
        {
            switch (RewardType)
            {
                case ItemRewardType.Weapon:
                    return new WeaponQuestReward(quest, Amount, WeaponOnAdd);
                case ItemRewardType.Item:
                default:
                    return new ItemQuestReward(quest, Item.Build(), Amount);
            }
        }
    }
}
