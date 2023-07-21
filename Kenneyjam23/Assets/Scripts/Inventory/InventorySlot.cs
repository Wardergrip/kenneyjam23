using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _image = null;
    [SerializeField] private TMP_Text _amount = null;

    public void ShowItem(Item item, int amount)
    {
        _image.sprite = item.ItemVisual.Sprite;
        _amount.text = amount.ToString();

        _image.gameObject.SetActive(true);
        _amount.transform.parent.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _image.gameObject.SetActive(false);
        _amount.transform.parent.gameObject.SetActive(false);
    }
}
