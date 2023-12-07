using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SP_Enemy_RestoreHealth : SpecialPowerOption
{
    public int shieldDiceSides;
    public int shieldBonusMultiplier;
    public int healthDiceSides;
    public int healthBonusMultiplier;
    public int baseHeavyShieldBonus;
    public int baseLightShieldBonus;
    public int baseHealthBonus;

    private Enemy enemy;
    private EnemyStatsUI enemyStatsUI;
    public override void PerformAction()
    {
        enemy = FindObjectOfType<Enemy>();
        enemyStatsUI = FindObjectOfType<EnemyStatsUI>();

        // restore shields
        if (enemy.currentHeavyArmor > 0 && enemy.currentLightShield > 0)
        {
            // if both armors exist, add to heavy armor
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseHeavyShieldBonus) * shieldBonusMultiplier;
            RestoreHeavyArmor(restoreAmount);
        } else if (enemy.currentHeavyArmor > 0 && enemy.currentLightShield <= 0)
        {
            // if heavy armor exists but light shield doesn't, restore light shield
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseLightShieldBonus) * shieldBonusMultiplier;
            RestoreLightShield(restoreAmount);
        } else if (enemy.currentLightShield > 0)
        {
            // if light shield exists but heavy armor doesn't, restore heavy armor
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseHeavyShieldBonus) * shieldBonusMultiplier;
            RestoreHeavyArmor(restoreAmount);
        } else
        {
            // if neither heavy armor nor light shield exist, restore light shield
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseLightShieldBonus) * shieldBonusMultiplier;
            RestoreLightShield(restoreAmount);
        }

        // restore health
        int restoreHealth = (Dice.DiceRoll(healthDiceSides) + baseHealthBonus) * healthBonusMultiplier;
        enemy.currentHealth += restoreHealth;
        Debug.Log($"enemy gained {restoreHealth} health");
        if (enemy.currentHealth > enemy.maxHealth)
        {
            Debug.Log($"Increased max health to {enemy.currentHealth}");
            enemy.maxHealth = enemy.currentHealth;
            enemyStatsUI.DisplayUpdatedHealth(enemy.currentHealth, enemy.currentHealth);
        }
        else
        {
            enemyStatsUI.DisplayHealth(enemy.currentHealth);
        }
    }

    private void RestoreLightShield(int amount)
    {
        Debug.Log($"enemy gained {amount} light shield");
        enemy.currentLightShield += amount;
        if (enemy.currentLightShield > enemy.maxLightShield)
        {
            Debug.Log($"Increased max light shield to {enemy.currentLightShield}");
            enemy.maxLightShield = enemy.currentLightShield;
            enemyStatsUI.DisplayUpdatedLightShield(enemy.currentLightShield, enemy.currentLightShield);
        }
        else
        {
            enemyStatsUI.DisplayLightShield(enemy.currentLightShield);
        }
    }

    private void RestoreHeavyArmor(int amount)
    {
        Debug.Log($"enemy gained {amount} heavy armor");
        enemy.currentHeavyArmor += amount;
        if (enemy.currentHeavyArmor > enemy.maxHeavyArmor)
        {
            Debug.Log($"Increased max heavy armor to {enemy.currentHeavyArmor}");
            enemy.maxHeavyArmor = enemy.currentHeavyArmor;
            enemyStatsUI.DisplayUpdatedHeavyArmor(enemy.currentHeavyArmor, enemy.currentHeavyArmor);
        }
        else
        {
            enemyStatsUI.DisplayHeavyArmor(enemy.currentHeavyArmor);
        }
    }
}
