using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_AddPermShield : Relic
{
    [Header("Relic Specific Values")]
    public int permShieldAmount;
    public override void AddRelicEffect(BattleSceneManager bsm, Player p)
    {
        battleSceneManager = bsm;
        topBar = battleSceneManager.topBar;
        player = p;
        player.playerRelics.Add(this);
        player.AddPermShield(permShieldAmount);
    }

    public override void RemoveRelicFromPlayer()
    {
        player.RemovePermShield(permShieldAmount);
        topBar.RemoveRelicItem(this);
        player.playerRelics.Remove(this);
    }
}
