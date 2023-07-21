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

    public bool IsFull { get { return Items.Count >= _amountOfItems; } }

    public bool AddItem(Item item, int amount = 1)
    {
        // If the items is already in the inventory, add to this stack
        if(Items.ContainsKey(item))
        {
            // Don't add an item if the stack of this item is already full
            if (Items[item] >= item.MaxStackSize) return false;

            Items[item] += amount; 
            InventoryUpdate.Invoke();
            return true;
        }

        // Don't add an item if the inventory is full
        if(IsFull)
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

    public Item WithdrawItem(Item item, int amount = 1)
    {
        // Return null if the item is not in the inventory
        if (!Items.ContainsKey(item))
        {
            return null;
        }

        // Remove item type from the inventory
        DeleteItem(item, amount);

        return item;
    }

    public bool DeleteItem(Item item, int amount = 1)
    {
        // Return false if the item is not in the inventory
        if (!Items.ContainsKey(item))
        {
            return false;
        }

        // Return false if the item does not contain enough items of this kind
        if (Items[item] < amount)
        {
            return false;
        }

        Items[item] -= amount;

        // Remove the item from the inventory if the amount reaches zero
        if (Items[item] == 0) Items.Remove(item);

        InventoryUpdate.Invoke();
        return true;
    }

    public bool DeleteItemStack(Item item)
    {
        // Return false if the item is not in the inventory
        if(!Items.ContainsKey(item))
        {
            return false;
        }

        // Delete all the items of this type
        Items.Remove(item);

        InventoryUpdate.Invoke();
        return true;
    }
}
