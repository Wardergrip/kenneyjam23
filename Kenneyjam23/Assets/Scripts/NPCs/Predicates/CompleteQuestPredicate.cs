using UnityEngine;

[CreateAssetMenu(menuName = "Dialog Predicates/CompleteQuestPredicate")]
public class CompleteQuest : DialogPredicate
{
    public Quest Quest = null;

    public override bool Validate()
    {
        return Quest.CanCompleteQuest();
    }
}