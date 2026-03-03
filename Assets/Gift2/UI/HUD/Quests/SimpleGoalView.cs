using Gift2.Core;
using TMPro;

namespace Gift2
{
    public class SimpleGoalView : QuestGoalView
    {
        public TextMeshProUGUI TextField;
        
        public override void Instantiate(Quest.QuestGoal goal)
        {
            TextField.SetText(goal.GetType().Name);
        }
    }
}
