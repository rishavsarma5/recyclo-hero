using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string cardTitle;
    public string cardSymbol;
    public string cardDescription;
    public int cardTier;
    public Sprite cardImage;
    public CardElement cardElement;

    public int GetCardTier()
    {
        return cardTier;
    }

    public CardElement GetCardElement()
    {
        return cardElement;
    }

    public string GetCardDescription()
    {
        return cardDescription;
    }

    public virtual void AddToInventory(BattleSceneManager battleSceneManager, CardUI cardUI)
    {
        battleSceneManager.itemsBought.Add(cardUI);
        battleSceneManager.coins -= GetCardTier();
        battleSceneManager.coinText.text = battleSceneManager.coins.ToString();
        cardUI.gameObject.SetActive(false);
        battleSceneManager.currentCards.Remove(this);
    }
}