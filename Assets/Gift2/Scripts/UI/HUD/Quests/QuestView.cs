using Gift2.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gift2
{
    public class QuestView : MonoBehaviour
    {
        public QuestGoalViewBuilder QuestGoalBuilder;
        public TextMeshProUGUI NameField;
        public TextMeshProUGUI DescriptionField;
        public Transform GoalsContainer;
        public QuestConfig TestQuestConfig;
        
        public Image Image;
        public Color OnReachedColor = Color.green;
        
        private Quest _quest;
        private Color _defaultColor;
    
        void Start()
        {
            if (_quest == null && TestQuestConfig != null)
            {
                var quest = TestQuestConfig.Build();
                quest.Accept(FindAnyObjectByType<Player>());
                Initialize(quest);
            }
            if (Image != null)
                _defaultColor = Image.color;
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
        
        void LateUpdate()
        {
            if (Image == null) return;
        
            if (_quest.GoalsIsReached())
                Image.color = OnReachedColor;
            else
                Image.color = _defaultColor;
        }
    }
}
