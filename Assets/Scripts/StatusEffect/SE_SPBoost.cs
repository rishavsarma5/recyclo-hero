using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class SE_SPBoost : StatusEffect
{
    public int spBonus;

    public override IEnumerator EvaluateEffect()
    {
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }

        target.isSPBoosted = true;

        switch (target) {
            case Player:
                battleSceneManager.playerStatusEffectSPChange += spBonus;
                break;
            case Enemy:
                battleSceneManager.playerStatusEffectSPChange += spBonus;
                break;
        }

        yield return new WaitForSeconds(0.5f);
    }

    public override void RemoveEffect()
    {
        Debug.Log($"removing status effect: {type}");
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }

        target.isSPBoosted = false;
        switch (target) {
            case Player:
                battleSceneManager.playerStatusEffectSPChange -= spBonus;
                break;
            case Enemy:
                battleSceneManager.playerStatusEffectSPChange -= spBonus;
                break;
        }
        target.activeStatusEffects.Remove(this);
    }
}
