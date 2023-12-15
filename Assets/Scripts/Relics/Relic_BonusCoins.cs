using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_BonusCoins : Relic
{
    [Header("Relic Specific Values")]
    public int coinBonus;
    public int relicBuffBonus;

    public override void AddRelicEffect(BattleSceneManager bsm, Player p)
    {
        battleSceneManager = bsm;
        topBar = battleSceneManager.topBar;
        player = p;
        player.playerRelics.Add(this);
        battleSceneManager.relicCoinsBonus = coinBonus + relicBuffBonus;
    }

    public override void RemoveRelicFromPlayer()
    {
        battleSceneManager.relicCoinsBonus = 0;
        topBar.RemoveRelicItem(this);
        player.playerRelics.Remove(this);
    }
}
