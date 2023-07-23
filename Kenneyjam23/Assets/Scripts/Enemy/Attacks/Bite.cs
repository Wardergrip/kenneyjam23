using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private void Start()
    {
        Debug.LogWarning("Implement damage to player here");
        PlayerController.Instance.GetComponent<Health>().Damage(_damage);
        Destroy(gameObject);
    }
}
