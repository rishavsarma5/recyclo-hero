using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoughtItemUI : MonoBehaviour, IDropHandler
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On Drop called");
        GameObject dropped = eventData.pointerDrag;
        //Debug.Log($"dropped value is: {dropped}");
        CardUI cardPicked = dropped.GetComponent<CardUI>();
        cardPicked.cardParentAfterDrag = transform;
        //Debug.Log($"cardui parent is: {cardPicked.transform.parent}");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    
}
