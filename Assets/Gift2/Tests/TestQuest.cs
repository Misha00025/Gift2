
using Gift2.Core;

namespace Gift2.Meta
{
    public class TestQuest : Quest
    {
        private class TestGoal : Quest.QuestGoal
        {
            public TestGoal(Quest quest) : base(quest)
            {
            }

            public override void Initialize()
            {
            }

            protected override bool CheckCondition()
            {
                return false;
            }
        }

        public TestQuest() : base("Test", "Description")
        {
            new TestGoal(this);
            new TestGoal(this);
            new TestGoal(this);
        }
    }
}