using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Relic : ScriptableObject
{
    [Header("Description")][Tooltip("Name")]
    public string relicName;

    public string relicDescription;

    [Header("Visuals")] 
    public Sprite relicImage;

    [Header("Misc")]
    public int turnDuration;
    [Tooltip("Set True if Relic is permanent (lasts whole game).")]
    public bool isPermanent;
    public BattleSceneManager battleSceneManager;
    public Player player;

    public void SetBattleSceneManager(BattleSceneManager bsm)
    {
        battleSceneManager = bsm;
    }

    public void DecreaseRelicTurnDuration()
    {
        if (!isPermanent)
        {
            turnDuration--;

            if (turnDuration == 0)
            {
                RemoveRelicFromPlayer();
            }
        }
    }

    public abstract void RemoveRelicFromPlayer();

    public abstract void AddEffect();
}
