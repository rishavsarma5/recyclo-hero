using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SE_CoinBoost : StatusEffect
{
    public int coinBonus;

    public override IEnumerator EvaluateEffect()
    {
        Debug.Log("coin boost evaluate effect triggered");
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }
        Debug.Log($"target: {target}");
        Debug.Log($"battle scene  manager: {battleSceneManager}");
        Debug.Log($"coin bonus: {coinBonus}");

        target.isCoinBoosted = true;
        battleSceneManager.waterSECoinBonus += coinBonus;
        yield return new WaitForSeconds(0.5f);
    }

    public override void RemoveEffect()
    {
        Debug.Log($"removing status effect: {type}");
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }

        target.isCoinBoosted = false;
        battleSceneManager.waterSECoinBonus -= coinBonus;
        target.activeStatusEffects.Remove(this);
    }
}
