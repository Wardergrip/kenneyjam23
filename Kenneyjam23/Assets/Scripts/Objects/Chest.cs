using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public static List<Chest> Chests { get; private set; } = new();

    [Header("Item management")]
    [SerializeField] private Item _item = null;
    [SerializeField] private float _refillTime = 120.0f;
    private float _curRefillTime = 0.0f;
    private static List<Item> _possibleItems = null;

    [Header("Fail message")]
    [SerializeField] private GameObject _failMessage = null;
    [SerializeField] private Transform _failMessageSocket = null;

    [Header("Refill timer message")]
    [SerializeField] private GameObject _timerMessage = null;
    [SerializeField] private TMP_Text _timerText = null;

    private void Start()
    {
        if (_possibleItems == null) _possibleItems = FindObjectOfType<GameManager>().ChestItems;

        // Make sure the refill timer is disabled by default
        _timerMessage.SetActive(false);

        // Choose a random item
        _item = _possibleItems[Random.Range(0, _possibleItems.Count)];
    }

    private void OnEnable() => Chests.Add(this);
    private void OnDisable() => Chests.Remove(this);

    private void Update()
    {
        // Nothing needs to happen if the chest has an item
        if (_item != null) return;

        // Decrease the refill time, when it reaches 0, restock the chest
        _curRefillTime -= Time.deltaTime;
        if (_curRefillTime <= 0.0f) Restock();

        UpdateRefillText();
    }

    private void UpdateRefillText()
    {
        _timerText.text = $"{(int)_curRefillTime / 60}:{(int)_curRefillTime % 60}";
    }

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

        // Enable the refill text
        _timerMessage.SetActive(true);
        _curRefillTime = _refillTime;

        return true;
    }

    private void Restock()
    {
        // Choose a random new item
        _item = _possibleItems[Random.Range(0, _possibleItems.Count)];

        // Play the close animation
        GetComponent<Animator>().SetTrigger("CloseChest");

        // Remove the refill text
        _timerMessage.SetActive(false);
    }
}
