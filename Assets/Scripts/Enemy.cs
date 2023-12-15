using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Target
{
    public List<EnemyUnit> enemies = new();
    public EnemyUnit currentEnemy { get; private set; }
    private EnemyAction currentAttack;
    public int turnNumber = 0;
    public GlobalInt currentHealth;
    public GlobalInt maxHealth;
    public GlobalInt currentLightShield;
    public GlobalInt maxLightShield;
    public GlobalInt currentHeavyArmor;
    public GlobalInt maxHeavyArmor;

    public GlobalBool enraged;
    public GlobalBool staggered;
    public int staggeredTurnCount = -1;

    public int lightShieldHealAmount = 2;

    [Range(0.0f, 1.0f)] public float heavyArmorDamageMultiplier = 0.8f;
    public int standardSpecialBarIncrease = 5;
    public int noHeavyArmorSpecialBarIncrease = 7;

    BattleSceneManager battleSceneManager;
    EnemyStatsUI enemyStatsUI;

    Player player;

    public AudioSource audioSource;

    //Animator animator;
    public bool enemyTurnOver;
    public bool attackRollOver;

    public Enemy(EnemyUnit currentEnemy)
    {
        this.currentEnemy = currentEnemy;
    }

    // Start is called before the first frame update
    void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        player = battleSceneManager.player;
        enemyStatsUI = FindObjectOfType<EnemyStatsUI>();
        
        //animator = GetComponent<Animator>();
    }

    public void SetupEnemy()
    {
        // Pick Enemy
        currentEnemy = enemies[Random.Range(0, enemies.Count)];
        
        // Set enemy HP Stats
        maxHealth.CurrentValue = currentEnemy.health;
        currentHealth.CurrentValue = currentEnemy.health;

        maxLightShield.CurrentValue = currentEnemy.lightShield;
        currentLightShield.CurrentValue = currentEnemy.lightShield;

        maxHeavyArmor.CurrentValue = currentEnemy.heavyArmor;
        currentHeavyArmor.CurrentValue = currentEnemy.heavyArmor;
        enemyStatsUI.healthSlider.maxValue = maxHealth.CurrentValue;
        enemyStatsUI.lightShieldSlider.maxValue = maxLightShield.CurrentValue;
        enemyStatsUI.heavyArmorSlider.maxValue = maxHeavyArmor.CurrentValue;
        enemyStatsUI.DisplayHealth(maxHealth.CurrentValue);
        enemyStatsUI.DisplayLightShield(maxLightShield.CurrentValue);
        enemyStatsUI.DisplayHeavyArmor(maxHeavyArmor.CurrentValue);
        enemyStatsUI.enemyName.text = currentEnemy.enemyName;
        enemyStatsUI.enemyImage.sprite = currentEnemy.enemyImage;
        audioSource.Stop();
        audioSource.clip = currentEnemy.bossMusic;
        audioSource.Play();
        RemoveAttack();
    }

    public IEnumerator AttackPlayer()
    {
        battleSceneManager.numTurns++;
        //animator.Play("Attack");
        EnemyAction ea = currentAttack;

        for (int i = 0; i < ea.numAttacks; i++)
        {
            yield return new WaitForSeconds(0.5f);
            int totalDamage = Dice.DiceRoll(ea.diceSides) + ea.baseDamage;
            totalDamage = (int)(enraged.CurrentValue ? totalDamage * currentEnemy.enragedMultiplier : totalDamage);
            battleSceneManager.rollButtonText.gameObject.SetActive(true);
            battleSceneManager.rollButtonText.text = "";
            battleSceneManager.rollButtonText.text = $"Enemy did {totalDamage} to player.";
            Debug.Log($"Enemy did {totalDamage} to player using {ea.actionName}.");
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
        if (currentHeavyArmor.CurrentValue > 0)
        {
            amount = (int)(amount * heavyArmorDamageMultiplier);
        }

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
        if (currentHeavyArmor.CurrentValue > 0)
        {
            if (currentHeavyArmor.CurrentValue > amount)
            {
                //block all
                Debug.Log($"heavy armor blocks all {amount} damage");
                currentHeavyArmor.CurrentValue -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"heavy armor blocks {currentHeavyArmor} damage");
                amount -= currentHeavyArmor.CurrentValue;
                currentHeavyArmor.CurrentValue = 0;
                staggered.CurrentValue = true;
                staggeredTurnCount = currentEnemy.staggeredTurns;
            }

            enemyStatsUI.DisplayHeavyArmor(currentHeavyArmor.CurrentValue);
        }

        return amount;
    }

    public int TakeLightShieldDamage(int amount)
    {
        if (amount > 0 && currentLightShield.CurrentValue > 0)
        {
            if (currentLightShield.CurrentValue > amount)
            {
                //block all
                Debug.Log($"light shield blocks rest {amount} damage");
                currentLightShield.CurrentValue -= amount;
                amount = 0;
            }
            else
            {
                //cant block all
                Debug.Log($"light shield blocks additional {currentLightShield} damage");
                amount -= currentLightShield.CurrentValue;
                currentLightShield.CurrentValue = 0;
                enraged.CurrentValue = true;
            }

            enemyStatsUI.DisplayLightShield(currentLightShield.CurrentValue);
        }

        return amount;
    }

    public void TakeHealthDamage(int amount)
    {
        if (amount > 0)
        {
            Debug.Log($"enemy health takes leftover {amount} damage");
            currentHealth.CurrentValue -= amount;
            enemyStatsUI.DisplayHealth(currentHealth.CurrentValue);
        }

        if (currentHealth.CurrentValue <= 0)
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
                currentHealth.CurrentValue -= amount;
                Debug.Log($"Enemy took {amount} corrosive damage to health");
                enemyStatsUI.DisplayHealth(currentHealth.CurrentValue);
                break;
            case CET_Water:
                currentLightShield.CurrentValue = Mathf.Max(0, currentLightShield.CurrentValue - amount);
                Debug.Log($"Enemy took {amount} water damage to light shield");
                enemyStatsUI.DisplayLightShield(currentLightShield.CurrentValue);
                break;
            case CET_Electricity:
                currentHeavyArmor.CurrentValue = Mathf.Max(0, currentHeavyArmor.CurrentValue - amount);
                Debug.Log($"Enemy took {amount} electric damage to heavy armor");
                enemyStatsUI.DisplayHeavyArmor(currentHeavyArmor.CurrentValue);
                break;
            default:
                Debug.Log("Should not get here");
                break;
        }

        if (currentHealth.CurrentValue <= 0)
        {
            Debug.Log("enemy dead");
            battleSceneManager.EndFight(true);
        }

        FinishAttackRoll();
    }

    public void AddLightShield(int amount)
    {
        currentLightShield.CurrentValue += amount;
        maxLightShield.CurrentValue += amount;
        enemyStatsUI.DisplayUpdatedLightShield(currentLightShield.CurrentValue, maxLightShield.CurrentValue);
    }

    public void AddHeavyArmor(int amount)
    {
        currentHeavyArmor.CurrentValue += amount;
        maxHeavyArmor.CurrentValue += amount;
        enemyStatsUI.DisplayUpdatedHeavyArmor(currentHeavyArmor.CurrentValue, maxHeavyArmor.CurrentValue);
    }

    private void FinishAttackRoll()
    {
        attackRollOver = true;
    }

    public void ReduceEnemyStaggered()
    {
        if (!staggered.CurrentValue) return;

        staggeredTurnCount--;
        if (staggeredTurnCount <= 0)
        {
            staggered.CurrentValue = false;
        }
    }

    public void LightShieldHeal()
    {
        if (currentLightShield.CurrentValue <= 0) return;

        if (currentHealth.CurrentValue < maxHealth.CurrentValue)
        {
            currentHealth.CurrentValue = Mathf.Min(maxHealth.CurrentValue,
                currentHealth.CurrentValue + lightShieldHealAmount);
            enemyStatsUI.DisplayHealth(currentHealth.CurrentValue);
        }
        else if (currentLightShield.CurrentValue < maxLightShield.CurrentValue)
        {
            currentLightShield.CurrentValue = Mathf.Min(maxLightShield.CurrentValue,
                currentLightShield.CurrentValue + lightShieldHealAmount);
            enemyStatsUI.DisplayLightShield(currentLightShield.CurrentValue);
        }
        else if (currentHeavyArmor.CurrentValue < maxHeavyArmor.CurrentValue)
        {
            currentHeavyArmor.CurrentValue = Mathf.Min(maxHeavyArmor.CurrentValue,
                currentHeavyArmor.CurrentValue + lightShieldHealAmount);
            enemyStatsUI.DisplayHeavyArmor(currentHeavyArmor.CurrentValue);
        }
        Debug.Log("Enemy Healed.");
    }

    public void RemoveAttack()
    {
        enemyStatsUI.enemyAttack.text = "";
        currentAttack = null;
    }

    public void GenerateAttack()
    {
        currentAttack = currentEnemy.NextEnemyAction();
        enemyStatsUI.enemyAttack.text = "Next Attack: " + currentAttack.actionName;
    }
}