using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autokill : MonoBehaviour
{
    [SerializeField] private float _time = 5f;
    private bool _isGettingDestroyed = false;

    // Update is called once per frame
    void Update()
    {
        if (_isGettingDestroyed) return;

        _time -= Time.deltaTime;

        if (_time <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() => _isGettingDestroyed = true; 
}
