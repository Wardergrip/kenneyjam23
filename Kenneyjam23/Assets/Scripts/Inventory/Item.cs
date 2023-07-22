using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public ItemVisual ItemVisual;
    public uint MaxStackSize = 1;
    public bool IsConsumable = false;

    public virtual void OnConsume() 
    {
        Debug.LogWarning($"OnConsume is not implemented on {name}, this is the default implementation. Keep in mind that IsConsumable {IsConsumable} is.");
    }
}
