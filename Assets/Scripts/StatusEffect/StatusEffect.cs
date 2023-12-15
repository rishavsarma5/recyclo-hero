using System.Collections;
using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public string type;
    public string description;
    public int turnDuration;
    public Target target;
    public Phase phase;
    public BattleSceneManager battleSceneManager;

    public abstract IEnumerator EvaluateEffect();

    public void SetTarget(Target _target)
    {
        target = _target;
    }

    public void SetBattleSceneManager(BattleSceneManager bsm)
    {
        battleSceneManager = bsm;
    }

    public void DecreaseTurn()
    {
        Debug.Log("decreasing turn in status effects");
        turnDuration--;
        Debug.Log($"decrease turn called, new turn duration: {turnDuration}");

        if (turnDuration <= 0)
        {
            RemoveEffect();
        }
    }

    public abstract void RemoveEffect();
}
