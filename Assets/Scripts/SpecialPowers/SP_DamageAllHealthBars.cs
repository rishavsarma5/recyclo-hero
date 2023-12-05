using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SP_DamageAllHealthBars : SpecialPowerOption
{
    public int diceSides;
    public int numAttacksPerHealthBar;

    private Enemy enemy;

    public void Start()
    {
        enemy = FindObjectOfType<Enemy>();
    }

    public override void PerformAction()
    {
        if (enemy.currentHeavyArmor != 0)
        {
            for (int i = 0; i < numAttacksPerHealthBar; i++)
            {
                int damage = Dice.DiceRoll(diceSides);
                Debug.Log($"dealing {damage} damage to enemy heavy armor");
                enemy.TakeHeavyArmorDamage(damage);
            }
        }

        if (enemy.currentLightShield != 0)
        {
            for (int i = 0; i < numAttacksPerHealthBar; i++)
            {
                int damage = Dice.DiceRoll(diceSides);
                Debug.Log($"dealing {damage} damage to enemy light shield");
                enemy.TakeLightShieldDamage(damage);
            }
        }

        if (enemy.currentHealth != 0)
        {
            for (int i = 0; i < numAttacksPerHealthBar; i++)
            {
                int damage = Dice.DiceRoll(diceSides);
                Debug.Log($"dealing {damage} damage to enemy health");
                enemy.TakeHealthDamage(damage);
            }
        }
    }
}
