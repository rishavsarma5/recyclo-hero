using UnityEngine;

[CreateAssetMenu]
public class SP_DamageAllHealthBars : SpecialPowerOption
{
    public int diceSides;
    public int numAttacksPerHealthBar;

    private Enemy enemy;

    public void Awake()
    {
        enemy = FindObjectOfType<Enemy>();
        Debug.Log("Enemy set in awake");
    }

    public override void PerformAction()
    {
        enemy = FindObjectOfType<Enemy>();
        if (enemy.currentHeavyArmor.CurrentValue != 0)
        {
            for (int i = 0; i < numAttacksPerHealthBar; i++)
            {
                int damage = Dice.DiceRoll(diceSides);
                Debug.Log($"dealing {damage} damage to enemy heavy armor");
                enemy.TakeHeavyArmorDamage(damage);
            }
        }

        if (enemy.currentLightShield.CurrentValue != 0)
        {
            for (int i = 0; i < numAttacksPerHealthBar; i++)
            {
                int damage = Dice.DiceRoll(diceSides);
                Debug.Log($"dealing {damage} damage to enemy light shield");
                enemy.TakeLightShieldDamage(damage);
            }
        }

        if (enemy.currentHealth.CurrentValue != 0)
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
