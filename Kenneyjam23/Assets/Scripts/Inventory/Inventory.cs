using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _amountOfItems = 5;
    public int MaxAmountOfItems { get { return _amountOfItems; } }

    public Dictionary<Item, int> Items { get; private set; } = new Dictionary<Item, int>();

    public UnityEvent InventoryUpdate = new UnityEvent();

    public bool AddItem(Item item, int amount = 1)
    {
        // If the items is already in the inventory, add another one
        if(Items.ContainsKey(item))
        {
            // Don't add an item if the stack is already full
            if (Items[item] >= item.MaxStackSize) return false;

            Items[item] += amount; 
            InventoryUpdate.Invoke();
            return true;
        }

        // Don't add an item if the inventory is full
        if(Items.Count >= _amountOfItems)
        {
            return false;
        }

        // Add a new item
        Items.Add(item, amount);
        InventoryUpdate.Invoke();
        return true;
    }

    public bool CheckForItem(Item item, int amount = 1)
    {
        // Return false if the item is not in the inventory
        if (!Items.ContainsKey(item)) return false;

        // Compare the amounts
        return Items[item] >= amount;
    }

    public Item WithdrawItem(Item item)
    {
        // Return null if the item is not in the inventory
        if (!Items.ContainsKey(item))
        {
            return null;
        }

        // Remove one of this item type from the inventory
        DeleteItem(item, false);

        return item;
    }

    public bool DeleteItem(Item item, bool deleteStack)
    {
        // Return false if the item is not in the inventory
        if(!Items.ContainsKey(item))
        {
            return false;
        }

        // Delete all the items of this type if deleteStack is true
        //  otherwise remove only one
        if(deleteStack)
        {
            Items.Remove(item);
        }
        else
        {
            --Items[item];

            // Delete the item if the amount reaches 0
            if (--Items[item] == 0) Items.Remove(item);
        }

        InventoryUpdate.Invoke();
        return true;
    }
}
