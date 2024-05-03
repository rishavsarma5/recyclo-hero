using UnityEngine;

[CreateAssetMenu]
public class CT_Weapon : Card
{
    public int baseDamage;
    public int diceSides;

    public int GetBaseDamage() => baseDamage;

    public int GetDiceSides() => diceSides;

    public override void AddToInventory(BattleSceneManager battleSceneManager, CardUI cardUI)
    {
        if (battleSceneManager.maxWeaponsBought <= 0) return;
        battleSceneManager.maxWeaponsBought--;

        base.AddToInventory(battleSceneManager, cardUI);
    }

    public override void RemoveFromInventory(BattleSceneManager battleSceneManager, CardUI cardUI)
    {
        //if (battleSceneManager.maxWeaponsBought <= 0) return;
        battleSceneManager.maxWeaponsBought++;

        base.RemoveFromInventory(battleSceneManager, cardUI);
    }
}