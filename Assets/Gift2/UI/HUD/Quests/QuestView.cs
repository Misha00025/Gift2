using Gift2.Core;
using Gift2.Meta;
using TMPro;
using UnityEngine;

namespace Gift2
{
    public class QuestView : MonoBehaviour
    {
        public QuestGoalViewBuilder QuestGoalBuilder;
        public TextMeshProUGUI NameField;
        public TextMeshProUGUI DescriptionField;
        public Transform GoalsContainer;
        
        private Quest _quest;
    
        void Start()
        {
            if (_quest == null)
                Initialize(new TestQuest());
        }
    
    
        public void Initialize(Quest quest)
        {
            _quest = quest;
            NameField.SetText(quest.Name);
            DescriptionField?.SetText(quest.Description);
            foreach (var goal in quest.Goals)
            {
                var view = QuestGoalBuilder.Build(goal, GoalsContainer);
            }
        }
        
        
    }
}
