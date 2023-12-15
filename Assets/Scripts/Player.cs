using System.Collections.Generic;
using UnityEngine;

public class Player : Target
{
    public int currentHealth;
    public int maxHealth;
    public int currentTempShield;
    public int maxTempShield;
    public int currentPermShield;
    public int maxPermShield;
    public List<TempShield> tempShields = new List<TempShield>();
    public bool applyShieldOver;
    public int baseCoinsBonus;
    public int relicCoinsBonus;

    public List<Relic> playerRelics = new List<Relic>();

    Enemy enemy;
    BattleSceneManager battleSceneManager;

    PlayerStatsUI playerStatsUI;
    // public GameObject damageIndicator;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        playerStatsUI = FindObjectOfType<PlayerStatsUI>();

        currentHealth = maxHealth;
        playerStatsUI.healthSlider.maxValue = maxHealth;
        playerStatsUI.DisplayHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        int newAmount = amount;
        Debug.Log($"player takes {newAmount} damage");
        int originalAmount = newAmount;
        
        if (currentTempShield > 0)
        {
            if (currentTempShield >= newAmount)
            {
                //block all
                Debug.Log($"temp shield blocks all {newAmount} damage");
                currentTempShield -= newAmount;
                newAmount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"temp shield blocks {currentTempShield} damage");
                newAmount -= currentTempShield;
                currentTempShield = 0;
            }

            if (currentTempShield <= 0)
            {
                playerStatsUI.tempShieldSlider.gameObject.SetActive(false);
                playerStatsUI.tempShieldSliderText.gameObject.SetActive(false);
                playerStatsUI.tempShieldImage.gameObject.SetActive(false);
            }
            else
            {
                playerStatsUI.DisplayTempShield(currentTempShield);
            }

            UpdateCurrentTempShields(originalAmount);
        }

        Debug.Log($"After temp shields, amount of damage left to block is {newAmount}");

        if (newAmount > 0 && currentPermShield > 0)
        {
            if (currentPermShield >= newAmount)
            {
                //block all
                Debug.Log($"perm shield blocks rest {newAmount} damage");
                currentPermShield -= newAmount;
                newAmount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"perm shield blocks additional {currentPermShield} damage");
                newAmount -= currentPermShield;
                currentPermShield = 0;
            }

            playerStatsUI.DisplayPermShield(currentPermShield);
        }

        Debug.Log($"player still takes {newAmount} damage to health");
        if (newAmount > 0)
        {
            currentHealth -= newAmount;
            playerStatsUI.DisplayHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("player dead");
            if (enemy != null)
            {
                battleSceneManager.EndFight(true);
                Die();
            }
            else
            {
                battleSceneManager.EndFight(false);
            }
        }
    }

    private void UpdateCurrentTempShields(int amount)
    {
        int numShieldsToRemove = 0;
        int newAmount = amount;

        for (int i = 0; i < tempShields.Count; i++)
        {
            if (tempShields[i].numShield >= newAmount)
            {
                if (tempShields[i].numShield - newAmount == 0)
                {
                    numShieldsToRemove++;
                    Debug.Log($"Decreasing max temp shield by {tempShields[i].numShield}");
                    maxTempShield -= tempShields[i].numShield;
                }
                tempShields[i].numShield -= newAmount;
                break;
            }
            else
            {
                //cant block all
                newAmount -= tempShields[i].numShield;
                Debug.Log($"Decreasing max temp shield by {tempShields[i].numShield}");
                maxTempShield -= tempShields[i].numShield;
                tempShields[i].numShield = 0;
                numShieldsToRemove++;
            }
        }

        for (int i = 0; i < numShieldsToRemove; i++)
        {
            tempShields.RemoveAt(0);
        }

        currentTempShield = 0;
        foreach (TempShield shield in tempShields)
        {
            currentTempShield += shield.numShield;
        }

        playerStatsUI.DisplayUpdatedTempShield(currentTempShield, maxTempShield);
    }

    public void UpdateCurrentTempShieldTurns()
    {
        List<int> indicesToRemove = new List<int>();
        for (int i = 0; i < tempShields.Count; i++)
        {
            tempShields[i].turnsLeft--;
            if (tempShields[i].turnsLeft <= 0)
            {
                Debug.Log($"temp shield {i} expired");
                Debug.Log($"Decreasing max temp shield by {tempShields[i].numShield}");
                maxTempShield -= tempShields[i].numShield;
                indicesToRemove.Add(i);
            }
            Debug.Log($"Temp shield {i} now has {tempShields[i].turnsLeft} turns left");
        }

        foreach (int i in indicesToRemove)
        {
            tempShields.RemoveAt(i);
        }

        currentTempShield = 0;
        foreach (TempShield shield in tempShields)
        {
            currentTempShield += shield.numShield;
        }

        if (currentTempShield <= 0)
        {
            playerStatsUI.tempShieldSlider.gameObject.SetActive(false);
            playerStatsUI.tempShieldSliderText.gameObject.SetActive(false);
            playerStatsUI.tempShieldImage.gameObject.SetActive(false);
        }
        else
        {
            playerStatsUI.DisplayUpdatedTempShield(currentTempShield, maxTempShield);
        }

        Debug.Log("temp shield turns updated");
    }

    public void AddTempShield(CT_Shield shield)
    {
        TempShield newTempShield = new(shield.GetTurns(), shield.GetDamageProtection());
        tempShields.Add(newTempShield);

        // TODO: Look into if this makes sense
        if (currentTempShield + newTempShield.numShield > maxTempShield)
        {
            currentTempShield += newTempShield.numShield;
            maxTempShield = currentTempShield;
        } else
        {
            currentTempShield += newTempShield.numShield;
        }

        playerStatsUI.tempShieldSlider.gameObject.SetActive(true);
        playerStatsUI.tempShieldSliderText.gameObject.SetActive(true);
        playerStatsUI.tempShieldImage.gameObject.SetActive(true);

        playerStatsUI.DisplayUpdatedTempShield(currentTempShield, maxTempShield);
        Debug.Log($"Added {shield.GetDamageProtection()} temp shield to player for {shield.GetTurns()} turns");
        Debug.Log($"updated temp shields to have {currentTempShield} current and {maxTempShield} max");
        applyShieldOver = true;
    }

    public void AddPermShield(int amount)
    {
        maxPermShield += amount;
        currentPermShield += amount;
        playerStatsUI.permShieldSlider.gameObject.SetActive(true);
        playerStatsUI.permShieldSliderText.gameObject.SetActive(true);
        playerStatsUI.permShieldImage.gameObject.SetActive(true);
        playerStatsUI.DisplayUpdatedPermShield(currentPermShield, maxPermShield);
        Debug.Log($"perm shield increased to {maxPermShield}");
    }

    public void RemovePermShield(int amount)
    {
        maxPermShield -= amount;
        currentPermShield -= amount;
        if (maxPermShield == 0)
        {
            playerStatsUI.permShieldSlider.gameObject.SetActive(false);
            playerStatsUI.permShieldSliderText.gameObject.SetActive(false);
            playerStatsUI.permShieldImage.gameObject.SetActive(false);
        } else
        {
            playerStatsUI.DisplayUpdatedPermShield(currentPermShield, maxPermShield);
        }
        Debug.Log($"perm shield decreased to {maxPermShield}");
    }

    public void RegeneratePermShield()
    {
        if (maxPermShield == 0) return;

        currentPermShield = maxPermShield;
        playerStatsUI.DisplayPermShield(currentPermShield);
        Debug.Log("perm shields regenerated");
    }

    private void Die()
    {
        this.gameObject.SetActive(false);
    }
}

public class TempShield
{
    public int turnsLeft;
    public int numShield;

    public TempShield(int turns, int shield)
    {
        turnsLeft = turns;
        numShield = shield;
    }
}