using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_AddThirdArm : Relic
{
    [Header("Relic Specific Values")]
    public int maxWeaponBonus;

    public override void AddRelicEffect(BattleSceneManager bsm, Player p)
    {
        battleSceneManager = bsm;
        topBar = battleSceneManager.topBar;
        player = p;
        player.playerRelics.Add(this);
        battleSceneManager.relicMaxWeaponsBonus = maxWeaponBonus;
    }

    public override void RemoveRelicFromPlayer()
    {
        battleSceneManager.relicMaxWeaponsBonus = 0;
        topBar.RemoveRelicItem(this);
        player.playerRelics.Remove(this);
    }
}
