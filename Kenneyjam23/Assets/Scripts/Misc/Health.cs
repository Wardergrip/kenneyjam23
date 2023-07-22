using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private int CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            _currentHealth = value;
            OnHealthChangedEvent?.Invoke(this);
        }
    }

    [Header("Events")]
    public UnityEvent<Health> OnHealthChangedEvent;
    public UnityEvent<Health> OnDamageReceivedEvent;
    public UnityEvent<Health> OnHealReceivedEvent;
    public UnityEvent<Health> OnDeathEvent;

    void Start()
    {
        Debug.Assert(_maxHealth > 0, "Max health is 0 or lower");
        CurrentHealth = _maxHealth;
    }

    public bool IsDead { get { return CurrentHealth <= 0; } }

    /// <summary>
    /// Returns true if the target died after applying the damage. Death event gets invoked if it dies with this damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool Damage(int damage)
    {
        if (IsDead) return true;

        CurrentHealth -= damage;
        OnDamageReceivedEvent?.Invoke(this);
        if (IsDead)
        {
            OnDeathEvent?.Invoke(this);
        }
        return IsDead;
    }

    /// <summary>
    /// Fails if target is dead. Return value indicates if it succeeded or not.
    /// </summary>
    /// <param name="amount"></param>
    public bool Heal(int amount)
    {
        if (IsDead) return false;
        CurrentHealth += amount;
        OnHealReceivedEvent?.Invoke(this);
        return true;
    }
}
