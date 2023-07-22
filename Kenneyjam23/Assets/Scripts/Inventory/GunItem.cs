using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunItem : Item
{
    public GameObject GunPrefab;

    public override void OnConsume()
    {
        Debug.Log("Code to equip the item comes here.");
    }
}
