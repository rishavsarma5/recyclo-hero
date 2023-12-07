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

    void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        enemyStatsUI = FindObjectOfType<EnemyStatsUI>();
        playerCurrentLevel = 0;
        enemyCurrentLevel = 0;
        playerMaxLevel = 12;
        enemyMaxLevel = 16;
        playerStatsUI.DisplayUpdatedSpecialPower(playerCurrentLevel, playerMaxLevel);
        enemyStatsUI.DisplayUpdatedSpecialPower(enemyCurrentLevel, enemyMaxLevel);
    }

    public IEnumerator IncreasePlayerPowerLevel(int amount)
    {
        if (playerCurrentLevel + amount >= playerMaxLevel)
        {
            playerStatsUI.DisplaySpecialPower(playerCurrentLevel + amount);
            StartCoroutine(battleSceneManager.DisplayPlayerSpecialAttackCards());
            yield return new WaitUntil(() => battleSceneManager.resetSpecialPowerLevel);
            battleSceneManager.resetSpecialPowerLevel = false;
            // wrap current Level back to 0 if it exceeds maxLevel
            playerCurrentLevel = (playerCurrentLevel + amount) - playerMaxLevel;
        } else
        {
            playerCurrentLevel += amount;
        }

        // update player's UI
        playerStatsUI.DisplaySpecialPower(playerCurrentLevel);
        yield return new WaitForSeconds(0.5f);
    }

    public void IncreaseEnemyPowerLevel(int amount)
    {
        if (enemyCurrentLevel + amount >= enemyMaxLevel)
        {
            StartCoroutine(battleSceneManager.PerformEnemySpecialPower());
            // wrap current Level back to 0 if it exceeds maxLevel
            enemyCurrentLevel = (enemyCurrentLevel + amount) - enemyMaxLevel;
        }
        else
        {
            enemyCurrentLevel += amount;
        }

        // update enemy's UI
        enemyStatsUI.DisplaySpecialPower(enemyCurrentLevel);
    }
}
