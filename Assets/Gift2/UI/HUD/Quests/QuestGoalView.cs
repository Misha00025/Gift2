using Gift2.Core;
using UnityEngine;

namespace Gift2
{
    public abstract class QuestGoalView : MonoBehaviour
    {
        public abstract void Instantiate(Quest.QuestGoal goal);
    }
    
    public abstract class QuestGoalView<T> : QuestGoalView where T : Quest.QuestGoal
    {
        protected T Goal { get; private set; }

        public override void Instantiate(Quest.QuestGoal goal)
        {
            if ((goal is T) == false) 
                throw new System.ArgumentException($"Type of goal is wrong. Expected: {typeof(T)}, received: {goal.GetType()}");
                
            Goal = (T)goal;
            OnInitialize();
        }
        
        protected abstract void OnInitialize();
    }
}
