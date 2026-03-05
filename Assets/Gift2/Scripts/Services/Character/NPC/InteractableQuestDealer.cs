using Gift2.Core;
using Gift2.Meta;
using UnityEngine;

namespace Gift2
{
    [RequireComponent(typeof(QuestDialer))]
    public class InteractableQuestDealer : Interactable
    {
        private QuestDialer _dealer;
        private Player _player;
        
        void Start()
        {
            _dealer = GetComponent<QuestDialer>();
            _player = FindAnyObjectByType<Player>();
        }
    
        public override void Use()
        {
            var quests = QuestsManager.Instance.GetDealerQuests(_dealer.Key);
            var ok = false;
            
            
            if (quests != null)            
                for (var i = quests.Count-1; i >= 0; i--)
                    ok = ok || _dealer.CompleteQuest(quests[i]);
            
            if (ok == false && (quests == null || quests.Count == 0))
                _dealer.AcceptQuest(_player);
        }
    }
}
