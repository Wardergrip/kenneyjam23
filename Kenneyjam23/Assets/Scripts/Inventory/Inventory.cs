using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _amountOfItems = 5;
    public Dictionary<Item, int> _items = new Dictionary<Item, int>();

    public bool AddItem(Item item)
    {
        // If the items is already in the inventory, add another one
        if(_items.ContainsKey(item))
        {
            ++_items[item];
            return true;
        }

        // Don't add an item if the inventory is full
        if(_items.Count >= _amountOfItems)
        {
            return false;
        }

        // Add a new item
        _items.Add(item, 1);
        return true;
    }

    public bool CheckForItem(Item item)
    {
        return _items.ContainsKey(item);
    }

    public Item WithdrawItem(Item item)
    {
        // Return null if the item is not in the inventory
        if (!_items.ContainsKey(item))
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
        if(!_items.ContainsKey(item))
        {
            return false;
        }

        // Delete all the items of this type if deleteStack is true
        //  otherwise remove only one
        if(deleteStack)
        {
            _items.Remove(item);
        }
        else
        {
            --_items[item];

            // Delete the item if the amount reaches 0
            if (--_items[item] == 0) _items.Remove(item);
        }

        return true;
    }
}
