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

    public bool TryCompleteQuest(Inventory inventory)
    {
        // Check if the player has the required items
        if (!inventory.CheckForItem(ItemRequired, AmountRequired))
            return false;

        // If the player has the item remove them from the inventory
        for (int i = 0; i < AmountRequired; ++i)
        {
            inventory.WithdrawItem(ItemRequired);
        }

        // Try to collect the quest's rewards
        TryCollectRewards(inventory);

        return true;
    }

    public void TryCollectRewards(Inventory inventory)
    {
        // Check if the player has room in their inventory
        if (true) // TODO: implement actual check
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
