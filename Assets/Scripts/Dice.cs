using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dice
{
    public static int DiceRoll(int sides)
    {
        return Random.Range(1, sides + 1);
    }
}
