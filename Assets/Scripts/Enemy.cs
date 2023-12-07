using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<EnemyAction> enemyActions;
    public List<EnemyAction> turns = new List<EnemyAction>();
    public int turnNumber = 0;
    public bool shuffleActions;
    public Enemy enemy;
    public int currentHealth;
    public int maxHealth;
    public int currentLightShield;
    public int maxLightShield;
    public int currentHeavyArmor;
    public int maxHeavyArmor;

    BattleSceneManager battleSceneManager;
    EnemyStatsUI enemyStatsUI;

    Player player;

    //Animator animator;
    public bool enemyTurnOver;
    public bool attackRollOver;

    // Start is called before the first frame update
    void Start()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        player = battleSceneManager.player;
        enemy = FindObjectOfType<Enemy>();
        enemyStatsUI = FindObjectOfType<EnemyStatsUI>();
        enemyStatsUI.healthSlider.maxValue = maxHealth;
        enemyStatsUI.lightShieldSlider.maxValue = maxLightShield;
        enemyStatsUI.heavyArmorSlider.maxValue = maxHeavyArmor;
        enemyStatsUI.DisplayHealth(maxHealth);
        enemyStatsUI.DisplayLightShield(maxLightShield);
        enemyStatsUI.DisplayHeavyArmor(maxHeavyArmor);
        //animator = GetComponent<Animator>();
        GenerateTurns();
    }
    public void GenerateTurns()
    {
        foreach (EnemyAction eA in enemyActions)
        {
            turns.Add(eA);
        }
        //turns.Shuffle();
    }

    public IEnumerator AttackPlayer()
    {
        battleSceneManager.numTurns++;
        //animator.Play("Attack");
        EnemyAction ea = turns[turnNumber];

        for (int i = 0; i < ea.numAttacks; i++)
        {
            yield return new WaitForSeconds(0.5f);
            int totalDamage = Dice.DiceRoll(ea.diceSides) + ea.baseDamage;
            battleSceneManager.rollButtonText.gameObject.SetActive(true);
            battleSceneManager.rollButtonText.text = "";
            battleSceneManager.rollButtonText.text = $"Enemy did {totalDamage} to player.";
            player.TakeDamage(totalDamage);
            yield return new WaitForSeconds(0.5f);
        }
        WrapUpTurn();
    }

    private void WrapUpTurn()
    {
        turnNumber = 0;
        
        if (battleSceneManager.numTurns % 6 == 0) // special attack
        {
            Debug.Log("special attack incoming!");
            turnNumber = 1;
        }
        Debug.Log($"Num turns is {battleSceneManager.numTurns} so turn number is {turnNumber}");
        enemyTurnOver = true;
    }

    public void TakeNormalDamage(int amount)
    {
        Debug.Log($"Enemy took {amount} normal damage");

        amount = TakeHeavyArmorDamage(amount);

        Debug.Log($"Damage left to block: {amount}");

        amount = TakeLightShieldDamage(amount);

        Debug.Log($"enemy health takes leftover {amount} damage");

        TakeHealthDamage(amount);
      
        FinishAttackRoll();
    }

    public int TakeHeavyArmorDamage(int amount)
    {
        if (currentHeavyArmor > 0)
        {
            if (currentHeavyArmor >= amount)
            {
                //block all
                Debug.Log($"heavy armor blocks all {amount} damage");
                currentHeavyArmor -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"heavy armor blocks {currentHeavyArmor} damage");
                amount -= currentHeavyArmor;
                currentHeavyArmor = 0;
            }
            enemyStatsUI.DisplayHeavyArmor(currentHeavyArmor);
        }

        return amount;
    }

    public int TakeLightShieldDamage(int amount)
    {
        if (amount > 0 && currentLightShield > 0)
        {
            if (currentLightShield >= amount)
            {
                //block all
                Debug.Log($"light shield blocks rest {amount} damage");
                currentLightShield -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"light shield blocks additional {currentLightShield} damage");
                amount -= currentLightShield;
                currentLightShield = 0;
            }
            enemyStatsUI.DisplayLightShield(currentLightShield);
        }

        return amount;
    }

    public void TakeHealthDamage(int amount)
    {
        if (amount > 0)
        {
            Debug.Log($"enemy health takes leftover {amount} damage");
            currentHealth -= amount;
            enemyStatsUI.DisplayHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("enemy dead");
            battleSceneManager.EndFight(true);
            Destroy(gameObject);
        }
    }
         
    public void TakeElementalDamage(Card card, int amount)
    {
        switch (card.GetCardElement())
        {
            case CET_Corrosion:
                currentHealth -= amount;
                Debug.Log($"Enemy took {amount} corrosive damage to health");
                enemyStatsUI.DisplayHealth(currentHealth);
                break;
            case CET_Water:
                currentLightShield = Mathf.Max(0, currentLightShield - amount);
                Debug.Log($"Enemy took {amount} water damage to light shield");
                enemyStatsUI.DisplayLightShield(currentLightShield);
                break;
            case CET_Electricity:
                currentHeavyArmor = Mathf.Max(0, currentHeavyArmor - amount);
                Debug.Log($"Enemy took {amount} electric damage to heavy armor");
                enemyStatsUI.DisplayHeavyArmor(currentHeavyArmor);
                break;
            default:
                Debug.Log("Should not get here");
                break;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("enemy dead");
            battleSceneManager.EndFight(true);
        }

        FinishAttackRoll();
    }

    public void AddLightShield(int amount)
    {
        currentLightShield += amount;
        maxLightShield += amount;
        enemyStatsUI.DisplayUpdatedLightShield(currentLightShield, maxLightShield);
    }

    public void AddHeavyArmor(int amount)
    {
        currentHeavyArmor += amount;
        maxHeavyArmor += amount;
        enemyStatsUI.DisplayUpdatedHeavyArmor(currentHeavyArmor, maxHeavyArmor);
    }

    private void FinishAttackRoll()
    {
        attackRollOver = true;
    }
}