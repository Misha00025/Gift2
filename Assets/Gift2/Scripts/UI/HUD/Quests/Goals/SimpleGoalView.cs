using Gift2.Core;
using TMPro;

namespace Gift2
{
    public class SimpleGoalView : QuestGoalView
    {
        public TextMeshProUGUI TextField;
        
        public override void Instantiate(Quest.QuestGoal goal)
        {
            TextField.SetText(GetText(goal));
        }
        
        protected virtual string GetText(Quest.QuestGoal goal)
        {
            return goal.GetType().Name;
        }
    }
}
