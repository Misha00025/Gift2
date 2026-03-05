using System.Collections.Generic;
using Gift2.Core;
using Gift2.Meta;
using UnityEngine;

namespace Gift2
{
    public class QuestsListView : MonoBehaviour
    {
        public QuestView ViewPrefab;
    
        private List<Quest> _quests = new();
        private Dictionary<Quest, QuestView> _views = new();
    
        void Start()
        {
            foreach (var quest in QuestsManager.Instance.Quests)
                AddQuest(quest);
        }
    
        void OnEnable()
        {
            QuestsManager.Instance?.QuestAdded.AddListener(AddQuest);
            QuestsManager.Instance?.QuestRemoved.AddListener(RemoveQuest);
        }
        
        void OnDisable()
        {
            QuestsManager.Instance?.QuestAdded.RemoveListener(AddQuest);
            QuestsManager.Instance?.QuestRemoved.RemoveListener(RemoveQuest);
        }
    
        public void AddQuest(Quest quest)
        {
            if (_quests.Contains(quest)) return;
            
            var view = Instantiate(ViewPrefab, transform);
            
            _quests.Add(quest);
            _views.Add(quest, view);
            
            view.Initialize(quest);
            quest.Completed.AddListener(RemoveQuest);
        }
        
        private void RemoveQuest(Quest quest)
        {
            if (_quests.Contains(quest) == false) return;
        
            _quests.Remove(quest);
            var view = _views[quest];
            
            Destroy(view.gameObject);
        }
    }
}
