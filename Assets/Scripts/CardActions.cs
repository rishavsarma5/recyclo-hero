using System.Collections;
using UnityEngine;

public class CardActions : MonoBehaviour
{
    public Player player;
    BattleSceneManager battleSceneManager;
    public bool clickedCardActionButton;
    public int diceSides;
    public int baseDamage;
    public int totalDamage;
    public bool isWeapon;
    public bool cardActionOver;
    public Target seSuccessTarget;


    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        seSuccessTarget = null;
    }
    public IEnumerator PerformAction(Card card, Enemy enemy)
    {
        battleSceneManager.cardActionButton.gameObject.SetActive(true);
        battleSceneManager.cardActionButtonText.gameObject.SetActive(true);
        cardActionOver = false;
        switch (card)
        {
            case CT_Weapon weapon:
                enemy.attackRollOver = false;
                StartCoroutine(NormalAttack(weapon, enemy));
                yield return new WaitUntil(() => enemy.attackRollOver);
                enemy.attackRollOver = false;
                StartCoroutine(ElementalAttack(weapon, enemy));
                yield return new WaitUntil(() => enemy.attackRollOver);
                seSuccessTarget = card.GetCardElement().DoSideEffect();
                if (seSuccessTarget)
                {
                    StartCoroutine(seSuccessTarget.activeStatusEffects[^1].EvaluateEffect());
                }
                break;
            case CT_Shield shield:
                player.applyShieldOver = false;
                StartCoroutine(ApplyShield(shield));
                yield return new WaitUntil(() => player.applyShieldOver);
                break;
        }



        cardActionOver = true;
        battleSceneManager.cardActionButton.gameObject.SetActive(true);
        battleSceneManager.cardActionButtonText.gameObject.SetActive(true);
    }

    public void CardActionButton()
    {
        if (isWeapon)
        {
            totalDamage = Dice.DiceRoll(diceSides) + baseDamage;
            Debug.Log($"Total damage rolled is {totalDamage}");
        }     
        clickedCardActionButton = true;
    }

    private IEnumerator NormalAttack(CT_Weapon weapon, Enemy enemy)
    {
        int baseDamage = weapon.GetBaseDamage();
        int diceSides = weapon.GetDiceSides();
        battleSceneManager.cardActionButtonText.text = $"Roll a d{diceSides} + {baseDamage} base damage for normal attack:";
        clickedCardActionButton = false;
        yield return new WaitUntil(() => clickedCardActionButton);
        int totalDamage = Dice.DiceRoll(diceSides) + baseDamage;
        enemy.TakeNormalDamage(totalDamage);
    }

    private IEnumerator ElementalAttack(CT_Weapon weapon, Enemy enemy)
    {
        clickedCardActionButton = false;
        battleSceneManager.cardActionButtonText.text = $"Roll a d{4} base damage for elemental attack:";
        yield return new WaitUntil(() => clickedCardActionButton);
        int totalDamage = Dice.DiceRoll(4);
        enemy.TakeElementalDamage(weapon, totalDamage);
    }

    private IEnumerator ApplyShield(CT_Shield shield)
    {
        battleSceneManager.cardActionButtonText.text = $"Add {shield.GetDamageProtection()} temp shield for {shield.GetTurns()} turns:";
        clickedCardActionButton = false;
        isWeapon = false;
        yield return new WaitUntil(() => clickedCardActionButton);
        player.AddTempShield(shield);
    }
}