using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class SE_SPDecrease : StatusEffect
{
    public int spDecrease;

    public override IEnumerator EvaluateEffect()
    {
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }

        target.isSPNerfed = true;
        switch (target)
        {
            case Player:
                battleSceneManager.playerStatusEffectSPChange -= spDecrease;
                break;
            case Enemy:
                battleSceneManager.enemyStatusEffectSPChange -= spDecrease;
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

        target.isSPNerfed = false;
        switch (target)
        {
            case Player:
                battleSceneManager.playerStatusEffectSPChange += spDecrease;
                break;
            case Enemy:
                battleSceneManager.enemyStatusEffectSPChange += spDecrease;
                break;
        }
        target.activeStatusEffects.Remove(this);
    }
}
