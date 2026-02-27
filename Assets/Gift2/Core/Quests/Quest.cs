using System.Collections.Generic;
using UnityEngine.Events;

namespace Gift2.Core
{
    public class Quest
    {
        public abstract class QuestGoal
        {
            private Quest _quest;
            
            public bool IsReached => CheckCondition();
            
            protected abstract bool CheckCondition();
            protected Quest Quest => _quest;
            
            public QuestGoal(Quest quest)
            {
                _quest = quest;
                _quest.AddGoal(this);
            }
            
            public abstract void Initialize();
        }
    
        public abstract class QuestReward 
        {
            private Quest _quest;
        
            public QuestReward(Quest quest)
            {
                _quest = quest;
                _quest.AddReward(this);
            }
        
            public abstract void Give();
        }
    
        public string Name { get; private set; } = "";
        public string Description { get; private set; } = "";
        
        private Player _player;
        private List<QuestGoal> _goals = new();
        private List<QuestReward> _rewards = new();
        
        public Player Player => _player;
        public IReadOnlyList<QuestGoal> Goals => _goals;
        public IReadOnlyList<QuestReward> Rewards => _rewards;
        public bool IsCanBeCompleted => CheckGoals();
        
        public UnityEvent<Quest> Completed { get; private set; } = new();
        
        
        public Quest(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
        
        public void Accept(Player player)
        {
            _player = player;
            foreach (var goal in _goals)
            {
                goal.Initialize();
            }
        }
        
        private void AddGoal(QuestGoal goal)
        {
            _goals.Add(goal);
        }
        
        private void AddReward(QuestReward reward)
        {
            _rewards.Add(reward);
        }
        
        public bool CheckGoals()
        {
            bool ok = true;
            foreach (var goal in _goals)
            {
                ok = ok && goal.IsReached;
            }
            return ok;
        }
        
        public void Complete()
        {
            foreach (var reward in _rewards)
            {
                reward.Give();
            }
            Completed.Invoke(this);
        }
    }
}
