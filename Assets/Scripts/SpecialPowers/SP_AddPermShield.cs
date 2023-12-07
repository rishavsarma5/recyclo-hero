using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SP_AddPermShield : SpecialPowerOption
{
    public int diceSides;
    public int shieldBuffMultiplier;

    private Player player;

    public void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public override void PerformAction()
    {
        int value = Dice.DiceRoll(diceSides);
        player = FindObjectOfType<Player>();
        player.AddPermShield(value * shieldBuffMultiplier);
    }
}
