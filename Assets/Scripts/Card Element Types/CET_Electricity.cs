using UnityEngine;

[CreateAssetMenu]
public class CET_Electricity : CardElement
{
    public override Target DoSideEffect()
    {
        Debug.Log("element electricty side effect procked!!!");
        if (!player || !enemy || !battleSceneManager) SetUnknownValues();

        float value = Dice.NormalizedDiceRoll();

        if (value <= successMax + relicBonus)
        {
            Debug.Log("Stun chance success!");
            SE_Stun stun = ScriptableObject.CreateInstance<SE_Stun>();
            stun.SetTarget(enemy);
            stun.SetBattleSceneManager(battleSceneManager);
            Debug.Log($"enemy in electric element is: {enemy}");
            enemy.activeStatusEffects.Add(stun);
            return enemy;
        } else
        {
            Debug.Log("Stun did not proc");
            return null;
        }
    }

    public override float ResistancePercentage()
    {
        throw new System.NotImplementedException();
    }
}