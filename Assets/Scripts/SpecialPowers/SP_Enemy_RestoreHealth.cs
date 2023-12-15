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
        if (enemy.currentHeavyArmor.CurrentValue > 0 && enemy.currentLightShield.CurrentValue > 0)
        {
            // if both armors exist, add to heavy armor
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseHeavyShieldBonus) * shieldBonusMultiplier;
            RestoreHeavyArmor(restoreAmount);
        }
        else if (enemy.currentHeavyArmor.CurrentValue > 0 && enemy.currentLightShield.CurrentValue <= 0)
        {
            // if heavy armor exists but light shield doesn't, restore light shield
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseLightShieldBonus) * shieldBonusMultiplier;
            RestoreLightShield(restoreAmount);
        }
        else if (enemy.currentLightShield.CurrentValue > 0)
        {
            // if light shield exists but heavy armor doesn't, restore heavy armor
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseHeavyShieldBonus) * shieldBonusMultiplier;
            RestoreHeavyArmor(restoreAmount);
        }
        else
        {
            // if neither heavy armor nor light shield exist, restore light shield
            int restoreAmount = (Dice.DiceRoll(shieldDiceSides) + baseLightShieldBonus) * shieldBonusMultiplier;
            RestoreLightShield(restoreAmount);
        }

        // restore health
        int restoreHealth = (Dice.DiceRoll(healthDiceSides) + baseHealthBonus) * healthBonusMultiplier;
        enemy.currentHealth.CurrentValue += restoreHealth;
        Debug.Log($"enemy gained {restoreHealth} health");
        if (enemy.currentHealth.CurrentValue > enemy.maxHealth.CurrentValue)
        {
            Debug.Log($"Increased max health to {enemy.currentHealth}");
            enemy.maxHealth = enemy.currentHealth;
            enemyStatsUI.DisplayUpdatedHealth(enemy.currentHealth.CurrentValue, enemy.currentHealth.CurrentValue);
        }
        else
        {
            enemyStatsUI.DisplayHealth(enemy.currentHealth.CurrentValue);
        }
    }

    private void RestoreLightShield(int amount)
    {
        Debug.Log($"enemy gained {amount} light shield");
        enemy.currentLightShield.CurrentValue += amount;
        if (enemy.currentLightShield.CurrentValue > enemy.maxLightShield.CurrentValue)
        {
            Debug.Log($"Increased max light shield to {enemy.currentLightShield}");
            enemy.maxLightShield = enemy.currentLightShield;
            enemyStatsUI.DisplayUpdatedLightShield(enemy.currentLightShield.CurrentValue,
                enemy.currentLightShield.CurrentValue);
        }
        else
        {
            enemyStatsUI.DisplayLightShield(enemy.currentLightShield.CurrentValue);
        }
    }

    private void RestoreHeavyArmor(int amount)
    {
        Debug.Log($"enemy gained {amount} heavy armor");
        enemy.currentHeavyArmor.CurrentValue += amount;
        if (enemy.currentHeavyArmor.CurrentValue > enemy.maxHeavyArmor.CurrentValue)
        {
            Debug.Log($"Increased max heavy armor to {enemy.currentHeavyArmor}");
            enemy.maxHeavyArmor = enemy.currentHeavyArmor;
            enemyStatsUI.DisplayUpdatedHeavyArmor(enemy.currentHeavyArmor.CurrentValue,
                enemy.currentHeavyArmor.CurrentValue);
        }
        else
        {
            enemyStatsUI.DisplayHeavyArmor(enemy.currentHeavyArmor.CurrentValue);
        }
    }
}