using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public static List<Chest> Chests { get; private set; } = new();

    [SerializeField] private Item _item = null;
    [SerializeField] private GameObject _failMessage = null;
    [SerializeField] private Transform _failMessageSocket = null;

    private void OnEnable() => Chests.Add(this);
    private void OnDisable() => Chests.Remove(this);
/// <summary>
/// Returns true if an interaction was possible (either item added to inventory or failed to add)
/// Returns false if internal item is null --> there is no item available to give.
/// </summary>
/// <returns></returns>
    public bool Interact(Inventory inventory)
    {
        if (_item == null) return false;

        if (!inventory.AddItem(_item))
        {
            // Spawn a message that the player's inventory is full
            Instantiate(_failMessage, _failMessageSocket.position, Quaternion.identity, null);
            return true;
        }

        // Play the open animation
        GetComponent<Animator>().SetTrigger("OpenChest");

        // Disable the chest by setting the item to null
        _item = null;

        return true;
    }

    public void Restock(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning($"Item that is passed to {gameObject.name} is null");
            return;
        }
        GetComponent<Animator>().SetTrigger("CloseChest");
        _item = item;
    }
}
