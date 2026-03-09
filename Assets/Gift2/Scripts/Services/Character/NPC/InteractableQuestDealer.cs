using Gift2.Core;
using Gift2.Meta;
using HeneGames.DialogueSystem;
using UnityEngine;
using Wof.DialogueSystem;

namespace Gift2
{
    [RequireComponent(typeof(QuestDialer))]
    public class InteractableQuestDealer : Interactable
    {
        public SentencesConfig SentencesConfig;
        public DialogueManager DialogueManager;
    
        private QuestDialer _dealer;
        private Player _player;
        private Quest _completedQuest;
        
        void Start()
        {
            _dealer = GetComponent<QuestDialer>();
            _player = FindAnyObjectByType<Player>();
            
        }
    
        public override void Use()
        {
            if (DialogueUI.instance.gameObject.activeSelf) return;
            
            var quests = QuestsManager.Instance.GetDealerQuests(_dealer.Key);

            if (quests == null || quests.Count == 0)
            {
                if (_dealer.Completed)
                {
                    DialogueManager.SetSentences(SentencesConfig.GetSentences(SentenceGroupType.NoQuests));
                }
                else
                {
                    DialogueManager.endDialogueEvent.AddListener(AcceptQuest);
                    DialogueManager.SetSentences(SentencesConfig.GetSentences(SentenceGroupType.AcceptQuest));    
                }
                DialogueManager.InitDialogue();
            }
            else if (quests[0].GoalsIsReached())
            {
                _completedQuest = quests[0];
                DialogueManager.endDialogueEvent.AddListener(CompleteQuest);
                DialogueManager.SetSentences(SentencesConfig.GetSentences(SentenceGroupType.CompleteQuest));
                DialogueManager.InitDialogue();
            }
            else 
            {
                DialogueManager.SetSentences(SentencesConfig.GetSentences(SentenceGroupType.InProgress));
                DialogueManager?.InitDialogue();
            }
        }
        
        private void AcceptQuest()
        {
            DialogueManager.endDialogueEvent.RemoveListener(AcceptQuest);
            _dealer.AcceptQuest(_player);
        }
        
        private void CompleteQuest()
        {
            if (_completedQuest == null) return;
        
            DialogueManager.endDialogueEvent.RemoveListener(CompleteQuest);
            _dealer.CompleteQuest(_completedQuest);
            _completedQuest = null;
        }
    }
}
