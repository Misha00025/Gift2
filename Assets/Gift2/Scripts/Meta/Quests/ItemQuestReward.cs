using Gift2.Core;
using Wof.InventoryManagement;

namespace Gift2.Meta
{
    public class ItemQuestReward : Quest.QuestReward
    {    
        private Item _item;
        private int _amount;
    
        public ItemQuestReward(Quest quest, Item item, int amount) : base(quest)
        {
            _item = item;
            _amount = amount;
        }

        public override void Give()
        {
            Quest.Player.Inventory.Add(_item, _amount);
        }
    }
}
