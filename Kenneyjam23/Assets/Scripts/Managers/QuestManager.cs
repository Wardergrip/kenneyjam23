using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField, Min(1)] private int _questsPerNPC = 1;

    private List<Quest> _quests = new List<Quest>();

    public int CurrentQuestIdx { get; private set; } = 0;

    public bool IsQuestlineCompleted { get; private set; } = false;

    public void HandleQuest(Inventory inventory, int questIdx)
    {
        // Check if the selected quest was already completed
        if (_quests[questIdx].IsCompleted)
        {
            // If so, try to claim the rewards
            TryCollectRewards(inventory, questIdx);
        }
        else
        {
            // Else, try to complete that quest
            TryCompleteQuest(inventory, questIdx);
        }
    }

    public void TryCompleteQuest(Inventory inventory, int questIdx)
    {
        // Try to complete the selected quest
        if (_quests[questIdx].TryCompleteQuest(inventory))
        {
            // If successful, unlock the next quest
            if (++CurrentQuestIdx == _questsPerNPC)
            {
                // If this was the last quest, mark questline as completed
                IsQuestlineCompleted = true;
            }
        }
    }

    public void TryCollectRewards(Inventory inventory, int questIdx)
    {
        // Try to collect the selected quest's rewards
        _quests[questIdx].TryCollectRewards(inventory);
    }
}
