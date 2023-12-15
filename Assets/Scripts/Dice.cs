using UnityEngine;

public static class Dice
{
    public static int DiceRoll(int sides)
    {
        return Random.Range(1, sides + 1);
    }

    public static float NormalizedDiceRoll()
    {
        return Random.Range(0f, 1f);
    }
}
