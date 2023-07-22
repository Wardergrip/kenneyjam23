using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 10;

    [Header("Events")]
    public UnityEvent<Transform> OnSpawnEvent;
    public UnityEvent<Transform> OnDestroyEvent;

    private void Start()
    {
        OnSpawnEvent?.Invoke(transform);
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherHealth = other.gameObject.GetComponent<Health>();
        if (otherHealth == null) return;

        otherHealth.Damage(_damage);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(transform);
    }
}
