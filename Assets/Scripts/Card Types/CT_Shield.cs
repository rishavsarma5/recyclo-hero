using UnityEngine;

[CreateAssetMenu]
public class CT_Shield : Card
{
    public int turns;
    public int damageProtection;

    public int GetTurns() => turns;
    public int GetDamageProtection() => damageProtection;

    public override void AddToInventory(BattleSceneManager battleSceneManager, CardUI cardUI)
    {
        if (battleSceneManager.maxArmorBought <= 0) return;
        battleSceneManager.maxArmorBought--;

        base.AddToInventory(battleSceneManager, cardUI);
    }

    public override void RemoveFromInventory(BattleSceneManager battleSceneManager, CardUI cardUI)
    {
        //if (battleSceneManager.maxArmorBought <= 0) return;
        battleSceneManager.maxArmorBought++;

        base.RemoveFromInventory(battleSceneManager, cardUI);
    }
}