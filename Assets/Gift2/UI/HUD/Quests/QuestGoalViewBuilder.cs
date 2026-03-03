using Gift2.Core;
using Gift2.Meta;
using UnityEngine;

namespace Gift2
{
    [CreateAssetMenu(fileName = "QuestGoalViewBuilder", menuName = "UI/QuestGoalViewBuilder")]
    public class QuestGoalViewBuilder : ScriptableObject
    {
        [field: SerializeField] public QuestGoalView DefaultGoalView { get; private set; }
        [field: SerializeField] public QuestGoalView<DeliverGoal> DeliverGoalView { get; private set; }
        public QuestGoalView Build(Quest.QuestGoal quest, Transform parent = null)
        {
            QuestGoalView prefab = null;
            switch (quest)
            {
                case DeliverGoal:
                    prefab = DeliverGoalView;
                    break;
                default:
                    prefab = DefaultGoalView;
                    break;
            }
            
            if (prefab == null)
                prefab = DefaultGoalView;
            var view = Instantiate(prefab, parent);
            view?.Instantiate(quest);
            return view;
        }
    }
}
