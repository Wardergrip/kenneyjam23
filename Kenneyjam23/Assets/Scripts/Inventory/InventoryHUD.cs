using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHUD : MonoBehaviour
{
    [Header("Components to listen to")]
    [SerializeField] private Inventory _inventory = null;

    [Header("HUD data")]
    [SerializeField] private GameObject _inventorySlot = null;
    [SerializeField] private float _distanceBetweenSlots = 5.0f;

    private List<InventorySlot> _inventorySlots = new List<InventorySlot>();

    private void Start()
    {
        int maxItems = _inventory.MaxAmountOfItems;

        float itemSize = GetComponent<RectTransform>().rect.height;
        float startPos = (-maxItems + 1) / 2.0f * itemSize - (maxItems - 1) / 2 * _distanceBetweenSlots;

        for(int i = 0; i < maxItems; ++i)
        {
            GameObject slotClone = Instantiate(_inventorySlot, transform);

            RectTransform slotTransform = slotClone.GetComponent<RectTransform>();
            slotTransform.localPosition = Vector3.right * (startPos + (itemSize + _distanceBetweenSlots) * i);
            slotTransform.sizeDelta = new Vector2(itemSize, itemSize);

            InventorySlot slot = slotClone.GetComponent<InventorySlot>();
            slot.Init(itemSize);

            _inventorySlots.Add(slot);
        }

        _inventory.InventoryUpdate.AddListener(() => UpdateInventory());

        UpdateInventory();
    }

    private void UpdateInventory()
    {
        var items = _inventory.Items;

        int curSlotIdx = 0;
        foreach(var itemPair in items)
        {
            _inventorySlots[curSlotIdx].ShowItem(itemPair.Key, itemPair.Value);

            ++curSlotIdx;
        }

        for(int i = curSlotIdx; i < _inventory.MaxAmountOfItems; ++i)
        {
            _inventorySlots[i].Hide();
        }
    }
}
