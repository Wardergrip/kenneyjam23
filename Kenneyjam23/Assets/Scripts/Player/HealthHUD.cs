using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHUD : MonoBehaviour
{
    [Header("Components to listen to")]
    [SerializeField] private Health _playerHealth = null;

    [Header("UI elements")]
    [SerializeField] private RectTransform _healthBar = null;

    private void Start()
    {
        // Subscribe to the playerchange event
        _playerHealth.OnHealthChangedEvent.AddListener(UpdateHealth);

        // Draw the initial value of the health on screen
        UpdateHealth(_playerHealth);
    }

    private void OnDestroy()
    {
        _playerHealth.OnHealthChangedEvent.RemoveListener(UpdateHealth);
    }

    public void UpdateHealth(Health health)
    {
        int curHealth = health.CurrentHealth;
        int maxHealth = health.MaxHealth;

        float healthPercentage = (float)curHealth / maxHealth;

        Rect parentRect = _healthBar.parent.GetComponent<RectTransform>().rect;

        _healthBar.sizeDelta = new Vector2(healthPercentage * parentRect.width, _healthBar.sizeDelta.y);
        _healthBar.anchoredPosition = Vector2.right * _healthBar.sizeDelta.x / 2.0f;
    }
}
