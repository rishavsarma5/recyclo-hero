using UnityEngine;

[CreateAssetMenu]
public class CET_Corrosion : CardElement
{
    public int decreaseSPAmount;
    public override Target DoSideEffect()
    {
        Debug.Log("element corrosion side effect procked!!!");
        if (!player || !enemy || !battleSceneManager) SetUnknownValues();

        float value = Dice.NormalizedDiceRoll();
        if (value <= successMax + relicBonus)
        {
            Debug.Log("Decrease SP side effect success!");
            SE_SPDecrease spDecrease = ScriptableObject.CreateInstance<SE_SPDecrease>();
            spDecrease.spDecrease = decreaseSPAmount;
            spDecrease.SetTarget(enemy);
            spDecrease.SetBattleSceneManager(battleSceneManager);
            enemy.activeStatusEffects.Add(spDecrease);
            return enemy;
        }
        else
        {
            Debug.Log("Decrease SP side effected not procked");
            return null;
        }
    }

    public override float ResistancePercentage()
    {
        throw new System.NotImplementedException();
    }
}