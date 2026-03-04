using Gift2.Meta;
using TMPro;
using UnityEngine.UI;

namespace Gift2
{
    public class DeliverGoalView : QuestGoalView<DeliverGoal>
    {
        public Image Image;
        public TextMeshProUGUI TextField;
        public string Prefix = "Соберите:";
        private string ItemsText => $"{Goal.Amount} {Goal.Item.Config.Name}";

    
        protected override void OnInitialize()
        {
            var text = $"{Prefix} {ItemsText}";
            if (Image != null)
                Image.sprite = Goal.Item.Config.Icon;
            TextField.SetText(text);
            OnApply();
        }
        
        void Update()
        {
            OnApply();
        }
        
        private void OnApply()
        {
            var text = $"{Prefix} {Goal.Current}/{ItemsText}";
            TextField.SetText(text);
        }
    }
}
