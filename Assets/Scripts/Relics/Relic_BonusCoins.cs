using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Relic_BonusCoins : Relic
{
    [Header("Relic Specific Values")]
    public int coinBonus;
    public int relicBuffBonus;

    public override void AddEffect()
    {
        battleSceneManager.relicCoinsBonus = coinBonus + relicBuffBonus;
    }

    public override void RemoveRelicFromPlayer()
    {
        battleSceneManager.relicCoinsBonus = 0;
        //player.playerRelics.Remove(this);
    }
}
