using System;
using System.Collections.Generic;
using HeneGames.DialogueSystem;
using UnityEngine;

namespace Wof.DialogueSystem
{
    [CreateAssetMenu(fileName = "SentencesConfig", menuName = "Dialogue System/New Sentences Config")]
    public class SentencesConfig : ScriptableObject
    {        
        [Serializable]
        public struct Sentence
        {
            public DialogueCharacter dialogueCharacter;

            [TextArea(3, 10)]
            public string sentence;
        }
        
        [Serializable]
        public class SentencesGroup
        {
            public List<Sentence> Sentences;
        }
        
        [SerializeField] private SentencesGroup AcceptQuest;
        [SerializeField] private SentencesGroup InProgress;
        [SerializeField] private SentencesGroup CompleteQuest;
        [SerializeField] private SentencesGroup NoQuests;
        
        public IReadOnlyList<Sentence> GetSentences(SentenceGroupType group)
        {
            switch (group)
            {
                case SentenceGroupType.AcceptQuest:
                    return AcceptQuest.Sentences;
                case SentenceGroupType.InProgress:
                    return InProgress.Sentences;
                case SentenceGroupType.CompleteQuest:
                    return CompleteQuest.Sentences;
                case SentenceGroupType.NoQuests:
                    return NoQuests.Sentences;
                default:
                    return NoQuests.Sentences;
            }
        }
    }
    
    public enum SentenceGroupType
    {
        AcceptQuest,
        InProgress,
        CompleteQuest,
        NoQuests    
    }
}
