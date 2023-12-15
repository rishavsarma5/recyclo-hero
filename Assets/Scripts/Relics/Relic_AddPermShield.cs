using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_AddPermShield : Relic
{
    [Header("Relic Specific Values")]
    public int permShieldAmount;
    public override void AddEffect()
    {
        player.AddPermShield(permShieldAmount);
    }

    public override void RemoveRelicFromPlayer()
    {
        player.RemovePermShield(permShieldAmount);
        //player.playerRelics.Remove(this);
    }
}
