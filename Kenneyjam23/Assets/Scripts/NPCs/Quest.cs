using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu()]
public class Quest : ScriptableObject
{
    [Header("Requirements")]
    public Item ItemRequired = null;
    [Min(1)] public int AmountRequired = 1;

    [Header("Rewards")]
    public Item ItemRewarded = null;
    [Min(1)] public int AmountRewarded = 1;

    public DialogBlock QuestDialog { get; set; } = null;

    [Header("Leave this empty if the quest can be used by any NPC")]
    public NPCType Type = null;

    public GameObject CompleteQuestFailMsg { private get; set; } = null;
    public GameObject CollectRewardsFailMsg { private get; set; } = null;
    public Transform FailMessageSocket { private get; set; } = null;

    private Inventory _playerInventory = null;
    public Inventory PlayerInventory
    {
        get { return _playerInventory; }
        private set { _playerInventory = value; }
    }

    private Inventory GetInventory()
    {
        // Get inventory
        if (PlayerInventory == null)
        {
            PlayerInventory = FindObjectOfType<Inventory>();
        }
        
        return PlayerInventory;
    }

    public bool CanCompleteQuest()
    {
        // Get inventory
        Inventory inventory = GetInventory();
        if (inventory == null)
        {
            CloseDialog(CompleteQuestFailMsg);

            return false;
        }

        bool isSuccess = inventory.CheckForItem(ItemRequired, AmountRequired);
        if (!isSuccess) CloseDialog(CompleteQuestFailMsg);

        return isSuccess;
    }

    public bool CanCollectReward()
    {
        // Get inventory
        Inventory inventory = GetInventory();
        if (inventory == null)
        {
            CloseDialog(CollectRewardsFailMsg);

            return false;
        }

        bool isSuccess = !(inventory.IsFull && inventory.CheckForItem(ItemRewarded, (int)ItemRewarded.MaxStackSize - AmountRewarded + 1));
        if (!isSuccess) CloseDialog(CollectRewardsFailMsg);

        return isSuccess;
    }

    private void CloseDialog(GameObject failMsg)
    {
        // Get dialog handler
        DialogHandler dialogHandler = FindObjectOfType<DialogHandler>();

        // Close dialog
        dialogHandler.CloseDialog();

        // Spawn a fail message
        Instantiate(failMsg, FailMessageSocket.position, Quaternion.identity, null);
    }

    public void CompleteQuest()
    {
        // Get inventory
        Inventory inventory = GetInventory();
        if (inventory == null) return;

        // Check if the player has the required items
        if (!CanCompleteQuest()) return;

        // If the player has the item remove them from the inventory
        inventory.WithdrawItem(ItemRequired, AmountRequired);

        // Update dialog to skip complet dialogs
        QuestDialog.NpcSpeech = $"Thanks for helping me earlier! Here are {AmountRewarded}x {ItemRewarded.ItemVisual.Name}.";
        QuestDialog.SetChoices(QuestDialog.Choices[0].Choices);
    }

    public void TryCollectRewards()
    {
        // Get inventory
        Inventory inventory = GetInventory();
        if (inventory == null) return;

        // Check if the player has room in their inventory
        if (!CanCollectReward()) return;

        // If so, add rewarded items
        inventory.AddItem(ItemRewarded, AmountRewarded);
    }
}
