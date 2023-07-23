using UnityEngine;

[CreateAssetMenu(menuName = "Dialog Predicates/CollectQuestPredicate")]
public class CollectQuest : DialogPredicate
{
    public Quest Quest = null;

    public override bool Validate()
    {
        return Quest.CanCollectReward();
    }
}