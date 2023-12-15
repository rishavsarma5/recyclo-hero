using System.Collections;
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

    public IEnumerator IncreasePlayerPowerLevel(int amount, int statusEffectChange)
    {
        if (playerCurrentLevel + amount + statusEffectChange >= playerMaxLevel)
        {
            playerStatsUI.DisplaySpecialPower(playerCurrentLevel + amount + statusEffectChange);
            StartCoroutine(battleSceneManager.DisplayPlayerSpecialAttackCards());
            yield return new WaitUntil(() => battleSceneManager.resetSpecialPowerLevel);
            battleSceneManager.resetSpecialPowerLevel = false;
            // wrap current Level back to 0 if it exceeds maxLevel
            playerCurrentLevel = (playerCurrentLevel + amount + statusEffectChange) - playerMaxLevel;
        } else
        {
            playerCurrentLevel += amount + statusEffectChange;
        }

        // update player's UI
        playerStatsUI.DisplaySpecialPower(playerCurrentLevel);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator IncreaseEnemyPowerLevel(int amount, int statusEffectChange)
    {
        if (enemyCurrentLevel + amount + statusEffectChange >= enemyMaxLevel)
        {
            enemyStatsUI.DisplaySpecialPower(enemyCurrentLevel + amount + statusEffectChange);
            StartCoroutine(battleSceneManager.PerformEnemySpecialPower());
            // wrap current Level back to 0 if it exceeds maxLevel
            enemyCurrentLevel = (enemyCurrentLevel + amount + statusEffectChange) - enemyMaxLevel;
        }
        else
        {
            enemyCurrentLevel += amount + statusEffectChange;
        }

        // update enemy's UI
        enemyStatsUI.DisplaySpecialPower(enemyCurrentLevel);
        yield return new WaitForSeconds(0.5f);
    }
}
