using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu()]
public class Quest : ScriptableObject
{
    public Item ItemRequired = null;
    [Min(1)] public int AmountRequired = 1;

    public Item ItemRewarded = null;
    [Min(1)] public int AmountRewarded = 1;

    public NPCType Type = null;

    public bool CanCompleteQuest(Inventory inventory)
    {
        return inventory.CheckForItem(ItemRequired, AmountRequired);
    }

    public bool TryCompleteQuest(Inventory inventory)
    {
        // Check if the player has the required items
        if (!CanCompleteQuest(inventory))
            return false;

        // If the player has the item remove them from the inventory
        inventory.WithdrawItem(ItemRequired, AmountRequired);

        // Try to collect the quest's rewards
        TryCollectRewards(inventory);

        return true;
    }

    public void TryCollectRewards(Inventory inventory)
    {
        // Check if the player has room in their inventory
        if (!inventory.IsFull)
        {
            // If so, add rewarded items
            for (int i = 0; i < AmountRewarded; ++i)
            {
                inventory.AddItem(ItemRewarded);
            }
        }
        else
        {
            // Else, notify player they can claim the reward later
            //...
        }
    }
}
