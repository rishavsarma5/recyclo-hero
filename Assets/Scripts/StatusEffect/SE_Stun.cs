using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class SE_Stun : StatusEffect
{
   public override IEnumerator EvaluateEffect()
    {
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }
        Debug.Log($"target: {target}");
        target.isStunned = true;
        yield return new WaitForSeconds(0.5f);
    }

    public override void RemoveEffect()
    {
        Debug.Log($"removing status effect: {type}");
        if (!target)
        {
            throw new MissingReferenceException("target not set on status effect!");
        }
        target.isStunned = false;
        target.activeStatusEffects.Remove(this);
    }
}
