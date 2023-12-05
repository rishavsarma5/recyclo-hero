using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SP_AddPermShield : SpecialPowerOption
{
    public int diceSides;
    public int shieldBuffMultiplier;

    private Player player;

    public void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public override void PerformAction()
    {
        int value = Dice.DiceRoll(diceSides);
        player.AddPermShield(value * shieldBuffMultiplier);
    }
}
