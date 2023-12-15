using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_AddThirdArm : Relic
{
    [Header("Relic Specific Values")]
    public int maxWeaponBonus;

    public override void AddEffect()
    {
        battleSceneManager.maxWeaponsBought += maxWeaponBonus;
    }

    public override void RemoveRelicFromPlayer()
    {
        battleSceneManager.maxWeaponsBought -= maxWeaponBonus;
        //player.playerRelics.Remove(this);
    }
}
