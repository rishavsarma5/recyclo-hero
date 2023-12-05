using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int currentTempShield;
    public int maxTempShield;
    public int currentPermShield;
    public int maxPermShield;
    public List<TempShield> tempShields = new List<TempShield>();
    public bool applyShieldOver;


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
        Debug.Log($"player takes {amount} damage");
        int originalAmount = amount;
        if (currentTempShield > 0)
        {
            if (currentTempShield >= amount)
            {
                //block all
                Debug.Log($"temp shield blocks all {amount} damage");
                currentTempShield -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"temp shield blocks {amount - currentTempShield} damage");
                amount -= currentTempShield;
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

        if (amount > 0 && currentPermShield > 0)
        {
            if (currentPermShield >= amount)
            {
                //block all
                Debug.Log($"perm shield blocks all {amount} damage");
                currentPermShield -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"perm shield blocks {currentPermShield - amount} damage");
                amount -= currentPermShield;
                currentPermShield = 0;
            }

            playerStatsUI.DisplayPermShield(currentPermShield);
        }

        Debug.Log($"player still takes {amount} damage to health");
        if (amount > 0)
        {
            currentHealth -= amount;
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

        for (int i = 0; i < tempShields.Count; i++)
        {
            if (tempShields[i].numShield >= amount)
            {
                tempShields[i].numShield -= amount;
                if (tempShields[i].numShield == 0)
                {
                    numShieldsToRemove++;
                }

                break;
            }
            else
            {
                //cant block all
                amount -= tempShields[i].numShield;
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

        playerStatsUI.DisplayTempShield(currentTempShield);
    }

    public void UpdateCurrentTempShieldTurns()
    {
        List<int> indicesToRemove = new List<int>();
        for (int i = 0; i < tempShields.Count; i++)
        {
            tempShields[i].turnsLeft--;
            if (tempShields[i].turnsLeft <= 0)
            {
                indicesToRemove.Add(i);
            }
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
            playerStatsUI.DisplayTempShield(currentTempShield);
        }

        Debug.Log("temp shield turns updated");
    }

    public void AddTempShield(CT_Shield shield)
    {
        TempShield newTempShield = new(shield.GetTurns(), shield.GetDamageProtection());
        tempShields.Add(newTempShield);

        currentTempShield += newTempShield.numShield;
        maxTempShield += newTempShield.numShield;

        playerStatsUI.tempShieldSlider.gameObject.SetActive(true);
        playerStatsUI.tempShieldSliderText.gameObject.SetActive(true);
        playerStatsUI.tempShieldImage.gameObject.SetActive(true);

        playerStatsUI.DisplayUpdatedTempShield(currentTempShield, maxTempShield);
        Debug.Log($"Added {shield.GetDamageProtection()} temp shield to player for {shield.GetTurns()}");
        Debug.Log($"updated temp shields to have {currentTempShield} current and {maxTempShield} max");
        applyShieldOver = true;
    }

    public void AddPermShield(int amount)
    {
        maxPermShield += amount;
        playerStatsUI.permShieldSlider.gameObject.SetActive(true);
        playerStatsUI.permShieldSliderText.gameObject.SetActive(true);
        playerStatsUI.permShieldImage.gameObject.SetActive(true);
        playerStatsUI.DisplayUpdatedPermShield(currentPermShield, maxPermShield);
        Debug.Log($"perm shield increased to {maxPermShield}");
    }

    public void RegeneratePermShield()
    {
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