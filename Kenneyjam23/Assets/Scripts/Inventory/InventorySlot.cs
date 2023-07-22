using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI elements")]
    [SerializeField] private Image _image = null;
    [SerializeField] private TMP_Text _amount = null;

    [Header("Name")]
    [SerializeField] private Image _nameImage = null;
    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private float _fadeSpeed = 1.0f;

    [Header("Usage menu")]
    [SerializeField] private GameObject _usageMenu = null;
    [SerializeField] private Button _useButton = null;

    private Color _nameColor;
    private bool _isHovering = false;

    private Inventory _inventory = null;
    private Item _item;

    public void Init(Inventory inventory, float size)
    {
        _inventory = inventory;

        RectTransform nameTransform = _nameImage.GetComponent<RectTransform>();
        nameTransform.sizeDelta = new Vector2(size, nameTransform.sizeDelta.y);
    }

    public void ShowItem(Item item, int amount)
    {
        _item = item;

        _useButton.interactable = item.IsConsumable;

        _image.sprite = item.ItemVisual.Sprite;
        _amount.text = amount.ToString();
        _name.text = item.ItemVisual.Name;

        _image.gameObject.SetActive(true);
        _amount.transform.parent.gameObject.SetActive(true);

        UpdateNameColor();
    }

    public void Hide()
    {
        _image.sprite = null;

        _image.gameObject.SetActive(false);
        _amount.transform.parent.gameObject.SetActive(false);

        ResetColor();

        ChangeUsage(false);
    }

    private void Start()
    {
        ResetColor();
    }

    private void Update()
    {
        if (_image.sprite == null) return;

        float fadeChange = _fadeSpeed * Time.deltaTime * (_isHovering ? 1.0f : -1.0f);

        if(_isHovering)
        {
            if (_nameColor.a > 1.0f) return;
        }
        else
        {
            if (_nameColor.a < 0.0f) return;
        }

        _nameColor.a += fadeChange;

        UpdateNameColor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        ChangeUsage(false);
    }

    private void ResetColor()
    {
        _nameColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        UpdateNameColor();
    }

    private void UpdateNameColor()
    {
        _nameImage.color = _nameColor;
        _name.color = _nameColor;
    }

    public void ChangeUsage(bool enabled)
    {
        _usageMenu.SetActive(enabled);
    }

    public void Use()
    {
        _item.OnConsume();
    }

    public void Delete()
    {
        _inventory.DeleteItem(_item);
    }

    public void TryUseItem()
    {
        if (_isHovering) Use();
    }
}
