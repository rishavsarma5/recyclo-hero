using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPower : MonoBehaviour
{
    public int playerCurrentLevel;
    public int enemyCurrentLevel;
    public int playerMaxLevel;
    public int enemyMaxLevel;

    BattleSceneManager battleSceneManager;

    PlayerStatsUI playerStatsUI;
    EnemyStatsUI enemyStatsUI;

    void Start()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        enemyStatsUI = FindObjectOfType<EnemyStatsUI>();
        playerCurrentLevel = 0;
        enemyCurrentLevel = 0;
        playerMaxLevel = 12;
        enemyMaxLevel = 18;
        playerStatsUI.spSlider.maxValue = playerMaxLevel;
        enemyStatsUI.spSlider.maxValue = enemyMaxLevel;
    }

    public void IncreasePlayerPowerLevel(int amount)
    {
        if (playerCurrentLevel + amount >= playerMaxLevel)
        {
            battleSceneManager.DisplayPlayerSpecialAttackCards();
            // wrap current Level back to 0 if it exceeds maxLevel
            playerCurrentLevel = playerMaxLevel - (playerCurrentLevel + amount);
        } else
        {
            playerCurrentLevel += amount;
        }

        // update player's UI
        playerStatsUI.DisplaySpecialPower(playerCurrentLevel);
    }

    public void IncreaseEnemyPowerLevel(int amount)
    {
        if (enemyCurrentLevel + amount >= enemyMaxLevel)
        {
            battleSceneManager.PerformEnemySpecialAttack();
            // wrap current Level back to 0 if it exceeds maxLevel
            enemyCurrentLevel = enemyMaxLevel - (enemyCurrentLevel + amount);
        }
        else
        {
            enemyCurrentLevel += amount;
        }

        // update enemy's UI
        enemyStatsUI.DisplaySpecialPower(enemyCurrentLevel);
    }
}
