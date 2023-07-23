using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HealthPack : Item
{
    public int HealAmount = 25;

    public override void OnConsume()
    {
        Health playerHealth = FindFirstObjectByType<PlayerController>().GetComponent<Health>();

        if(playerHealth != null)
        {
            playerHealth.Heal(HealAmount);
        }
    }
}
