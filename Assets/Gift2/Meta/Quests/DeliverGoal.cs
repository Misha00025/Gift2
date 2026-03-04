using Gift2.Core;

namespace Gift2.Meta
{
    public class DeliverGoal : Quest.QuestGoal
    {
        public struct DeliverData 
        {
            public Item Item { get; private set; }
            public int Amount { get; private set; }
            
            public DeliverData(Item item, int amount)
            {
                Item = item;
                Amount = amount;
            }
        }

        private DeliverData _data;

        public DeliverGoal(Quest quest, DeliverData data) : base(quest)
        {
            _data = data;
        }

        public Item Item => _data.Item;
        public int Amount => _data.Amount;
        public int Current => Quest.Player?.ResourcesStorage?.Count(Item) ?? 0;

        public override void Initialize()
        {
            Quest.Completed.AddListener(OnComplete);
        }

        private void OnComplete(Quest quest)
        {
            quest.Player.ResourcesStorage.Remove(_data.Item, _data.Amount);
        }

        protected override bool CheckCondition()
        {
            var count = Quest.Player.ResourcesStorage.Count(_data.Item);
            return count > _data.Amount;
        }
    }
}
