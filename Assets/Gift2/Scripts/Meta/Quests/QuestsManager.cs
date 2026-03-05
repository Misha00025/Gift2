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
        private Dictionary<string, List<Quest>> _dialerQuests = new();
        
        public IReadOnlyList<Quest> Quests => _quests;
        
        public UnityEvent<Quest> QuestAdded { get; private set; } = new();
        public UnityEvent<Quest> QuestRemoved { get; private set; } = new();
        
        public void AddQuest(Quest quest, QuestDialer dialer)
        {
            if (_quests.Contains(quest)) return;

            _quests.Add(quest);
            var key = dialer.Key;
            _dialerKeys.Add(quest, key);
            if (_dialerQuests.ContainsKey(key) == false)
                _dialerQuests.Add(key, new());
            _dialerQuests[key].Add(quest);
            quest.Completed.AddListener(RemoveQuest);
            QuestAdded.Invoke(quest);
        }
        
        public string GetDealerKey(Quest quest)
        {
            if (_dialerKeys.ContainsKey(quest) == false) return "";
            
            return _dialerKeys[quest];
        }
        
        public IReadOnlyList<Quest> GetDealerQuests(string key)
        {
            if (_dialerQuests.ContainsKey(key) == false) return null;
            
            return _dialerQuests[key];
        }
        
        public void RemoveQuest(Quest quest)
        {
            var ok = _quests.Remove(quest);
            if (ok == false) return;
            
            var key = _dialerKeys[quest];
            _dialerKeys.Remove(quest);
            _dialerQuests[key].Remove(quest);
            
            QuestRemoved.Invoke(quest);
        }
        
        public static void Clear()
        {
            _instance = null;
        }
    }
}
