using UnityEngine;

[CreateAssetMenu]
public class CET_Water : CardElement
{
    public override Target DoSideEffect()
    {
        Debug.Log("element water side effect procked!!!");
        if (!player || !enemy || !battleSceneManager) SetUnknownValues();

        float value = Dice.NormalizedDiceRoll();
        if (value <= successMax + relicBonus)
        {
            Debug.Log("Water bonus coins success!");
            SE_CoinBoost waterCoinBoost = ScriptableObject.CreateInstance<SE_CoinBoost>();
            waterCoinBoost.SetTarget(player);
            waterCoinBoost.SetBattleSceneManager(battleSceneManager);
            waterCoinBoost.coinBonus = 2;
            player.activeStatusEffects.Add(waterCoinBoost);
            return player;
        }
        else
        {
            Debug.Log("Water bonus not proc");
            return null;
        }
    }

    public override float ResistancePercentage()
    {
        throw new System.NotImplementedException();
    }
}