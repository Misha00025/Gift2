using Gift2.Core;
using Gift2.Meta;
using HeneGames.DialogueSystem;
using UnityEngine;

namespace Gift2
{
    [RequireComponent(typeof(QuestDialer))]
    public class InteractableQuestDealer : Interactable
    {
        public DialogueManager StartDialogueManager;
        public DialogueManager InProgressDialogueManager;
        public DialogueManager EndDialogueManager;
    
        private QuestDialer _dealer;
        private Player _player;
        private Quest _completedQuest;
        
        void Start()
        {
            _dealer = GetComponent<QuestDialer>();
            _player = FindAnyObjectByType<Player>();
            StartDialogueManager?.endDialogueEvent.AddListener(AcceptQuest);
            EndDialogueManager?.endDialogueEvent.AddListener(CompleteQuest);
            
        }
    
        public override void Use()
        {
            if (DialogueUI.instance.gameObject.activeSelf) return;
            
            var quests = QuestsManager.Instance.GetDealerQuests(_dealer.Key);

            if (quests == null || quests.Count == 0)
            {
                StartDialogueManager?.InitDialogue();
            }
            else if (quests[0].GoalsIsReached())
            {
                _completedQuest = quests[0];
                EndDialogueManager?.InitDialogue();
            }
            else 
            {
                InProgressDialogueManager?.InitDialogue();
            }
        }
        
        private void AcceptQuest()
        {
            _dealer.AcceptQuest(_player);
        }
        
        private void CompleteQuest()
        {
            if (_completedQuest == null) return;
        
            _dealer.CompleteQuest(_completedQuest);
            _completedQuest = null;
        }
    }
}
