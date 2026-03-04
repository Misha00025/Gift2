using Gift2.Core;
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
        public QuestConfig TestQuestConfig;
        
        private Quest _quest;
    
        void Start()
        {
            if (_quest == null && TestQuestConfig != null)
            {
                var quest = TestQuestConfig.Build();
                quest.Accept(FindAnyObjectByType<Player>());
                Initialize(quest);
            }
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
