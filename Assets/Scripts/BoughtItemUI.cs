using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoughtItemUI : MonoBehaviour, IDropHandler
{
    Vector2 anchoredPosition;
    public CardUI cardInSlot;
    public string allowedCardType;
    BattleSceneManager battleSceneManager;

    private void Awake()
    {
        anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(this + " anchored position is: " + anchoredPosition);
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        cardInSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop On " + this + " called now");
        GameObject dropped = eventData.pointerDrag;
        CardUI temp = dropped.GetComponent<CardUI>();
        //Debug.Log("What is temp: " + temp);
        //Debug.Log("What is cardslot: " + this.cardInSlot);
        if (!this.cardInSlot) // no card currently in slot
        {
            Debug.Log("going to first if");
            if (temp.card.cardSymbol == allowedCardType && temp.boughtItemSlotParent) // move card from bought slot to empty slot
            {
                temp.boughtItemSlotParent.cardInSlot = null;
                temp.boughtItemSlotParent = null;
                cardInSlot = temp;
                cardInSlot.boughtItemSlotParent = this;
                cardInSlot.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            } else if (temp.card.cardSymbol == allowedCardType && !temp.boughtItemSlotParent) // move card from list to bought slot
            {
                cardInSlot = temp;
                cardInSlot.boughtItemSlotParent = this;
                cardInSlot.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
                cardInSlot.card.AddToInventory(battleSceneManager, cardInSlot);
            } else if (temp.boughtItemSlotParent) // move card back to old slot since card type doesn't match bought slot
            {
                temp.GetComponent<RectTransform>().anchoredPosition = temp.boughtItemSlotParent.anchoredPosition;
                temp.StartCoroutine(temp.DelayedDeselectCard());
            }
        } 
    }

    public void PerformSwap(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        CardUI temp = dropped.GetComponent<CardUI>();

        Debug.Log("Trying to swap");
        Debug.Log("What is temp: " + temp);
        Debug.Log("what is undercard: " + cardInSlot);
        if (temp.card.cardSymbol == allowedCardType && temp.boughtItemSlotParent) // you can swap with current card in slot
        {
            /*
            CardUI currCard = cardInSlot;
            BoughtItemUI currSlot = cardInSlot.boughtItemSlotParent;

            cardInSlot.boughtItemSlotParent = temp.boughtItemSlotParent;
            cardInSlot.GetComponent<RectTransform>().anchoredPosition = temp.GetComponent<RectTransform>().anchoredPosition;

            temp.boughtItemSlotParent = currSlot;
            temp.GetComponent<RectTransform>().anchoredPosition = currSlot.GetComponent<RectTransform>().anchoredPosition;

            cardInSlot = temp;
            currCard.boughtItemSlotParent.cardInSlot = currCard;
            */
            
            // swap card in current slot first
            this.cardInSlot.GetComponent<RectTransform>().anchoredPosition = temp.boughtItemSlotParent.anchoredPosition;
            this.cardInSlot.boughtItemSlotParent = temp.boughtItemSlotParent;
            temp.boughtItemSlotParent.cardInSlot = this.cardInSlot;
            // swap temp card into current slot
            this.cardInSlot = temp;
            this.cardInSlot.boughtItemSlotParent = this;
            this.cardInSlot.GetComponent<RectTransform>().anchoredPosition = this.anchoredPosition;
            //this.cardInSlot.card.AddToInventory(battleSceneManager, cardInSlot);
            
        }
        else if (temp.card.cardSymbol == allowedCardType && !temp.boughtItemSlotParent) // card from list tries to move onto current bought card slot
        {
            temp.GetComponent<RectTransform>().anchoredPosition = temp.originalPosition;
        }
        else if (temp.boughtItemSlotParent) // move card back to old slot
        {
            temp.GetComponent<RectTransform>().anchoredPosition = temp.boughtItemSlotParent.anchoredPosition;
            temp.StartCoroutine(temp.DelayedDeselectCard());
        }
    }

    
}
