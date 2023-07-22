using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
    private void Start()
    {
        Debug.LogWarning("Implement damage to player here");
        Destroy(gameObject);
    }
}
