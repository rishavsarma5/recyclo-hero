using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoughtItemUI : MonoBehaviour, IDropHandler
{
    Vector2 anchoredPosition;
    public CardUI cardInSlot;
    BattleSceneManager battleSceneManager;

    private void Awake()
    {
        anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop On BoughtItemUI called now");
        GameObject dropped = eventData.pointerDrag;
        Debug.Log("on drop game object: " + dropped);
        cardInSlot = dropped.GetComponent<CardUI>();
        Debug.Log("on drop card ui : " + cardInSlot);
        cardInSlot.boughtItemSlotParent = this;
        //cardPicked.cardParentAfterDrag = this.transform;
        cardInSlot.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        cardInSlot.card.AddToInventory(battleSceneManager, cardInSlot);
        Debug.Log("BattleSceneManager in boughtUI bought Items List Count: " + battleSceneManager.itemsBought.Count);
    }

    
}
