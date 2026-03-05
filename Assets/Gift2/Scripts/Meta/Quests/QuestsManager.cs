using System.Collections.Generic;
using Gift2.Core;
using UnityEngine.Events;

namespace Gift2.Meta
{
    public class QuestsManager
    {
        private static QuestsManager _instance;
    
        public static QuestsManager Instance { 
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }
        
        private List<Quest> _quests = new();
        private Dictionary<Quest, string> _dialerKeys = new();
        
        public IReadOnlyList<Quest> Quests => _quests;
        
        public UnityEvent<Quest> QuestAdded { get; private set; } = new();
        public UnityEvent<Quest> QuestRemoved { get; private set; } = new();
        
        public void AddQuest(Quest quest, QuestDialer dialer)
        {
            if (_quests.Contains(quest)) return;

            _quests.Add(quest);
            _dialerKeys.Add(quest, dialer.Key);
            quest.Completed.AddListener(RemoveQuest);
            QuestAdded.Invoke(quest);
        }
        
        public string GetDealerKey(Quest quest)
        {
            if (_dialerKeys.ContainsKey(quest) == false) return "";
            
            return _dialerKeys[quest];
        }
        
        public void RemoveQuest(Quest quest)
        {
            var ok = _quests.Remove(quest);
            if (ok == false) return;
            
            _dialerKeys.Remove(quest);
            QuestRemoved.Invoke(quest);
        }
        
        public static void Clear()
        {
            _instance = null;
        }
    }
}
