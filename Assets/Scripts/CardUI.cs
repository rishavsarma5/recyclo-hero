using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IDropHandler, IPointerEnterHandler
{
    public Card card;
    public TMP_Text cardTitleText;
    public TMP_Text cardDescriptionText;
    public TMP_Text cardTierText;
    public TMP_Text cardElementText;
    public TMP_Text cardTypeText;

    public Image cardImage;
    public Image cardTierImage;
    public Transform cardParentMenu;
    public BoughtItemUI boughtItemSlotParent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool pointerOnCard = false;
    public Vector2 originalPosition;
    private bool hasBeenDragged = false;
    public BoughtItemUI hoveredSlot;

    //public GameObject discardEffect;
    BattleSceneManager battleSceneManager;
    private Animator animator;

    // Animator animator;
    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();
        originalPosition = rectTransform.anchoredPosition;
        this.rectTransform.anchoredPosition = originalPosition;
        Debug.Log("original position:" + originalPosition);
        cardParentMenu = transform.parent;
    }

    public void LoadCard(Card _card)
    {
        card = _card;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        cardTitleText.text = card.cardTitle;
        cardDescriptionText.text = card.GetCardDescription();
        cardTierText.text = card.GetCardTier().ToString();
        cardElementText.text = card.GetCardElement().elementName;
        cardTypeText.text = card.cardSymbol;
        cardImage.sprite = card.cardImage;

        switch (card.GetCardTier())
        {
            case 1:
                cardTierImage.color = Color.green;
                break;
            case 2:
                cardTierImage.color = Color.cyan;
                break;
            case 3:
                cardTierImage.color = Color.yellow;
                break;
            case 4:
                cardTierImage.color = Color.magenta;
                break;
            default:
                Debug.Log("should not have a tier that is this.");
                break;
        }
    }

    public void SelectCard()
    {
        //Debug.Log("card is selected");
        hasBeenDragged = false;
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (boughtItemSlotParent) return;
        
        if (battleSceneManager.coins - card.GetCardTier() < 0)
        {
            // Cancel the potential drag
            eventData.pointerDrag = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag called on " + this);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        hasBeenDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag called on " + this);
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag called on " + this);

        if (!boughtItemSlotParent) // if card not moved to a bought item slot
        {
            this.rectTransform.anchoredPosition = originalPosition;
            canvasGroup.blocksRaycasts = true;
        } else // card was moved to a bought item slot
        {
            canvasGroup.blocksRaycasts = true;
        }
        StartCoroutine(DelayedDeselectCard());
    }

    public void DeselectCard()
    {
        Debug.Log("On deselect called on " + this);
        battleSceneManager.selectedCard = null;
        if (boughtItemSlotParent && !hasBeenDragged) { 
            boughtItemSlotParent.cardInSlot = null;
            transform.SetParent(transform.parent);
            this.rectTransform.anchoredPosition = originalPosition;
            boughtItemSlotParent = null;
            card.RemoveFromInventory(battleSceneManager, this);
        }
    }

    public IEnumerator DelayedDeselectCard()
    {
        yield return new WaitForEndOfFrame(); // Wait until the end of the frame
        DeselectCard(); // Deselect the card
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (hoveredSlot == null)
            return;

        hoveredSlot.PerformSwap(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverCard(eventData);
    }

    public void HoverCard(PointerEventData pointerEventData)
    {
        if (!pointerOnCard)
        {
            animator.Play("CardUIOnHover");
            Debug.Log("hover on called");
            pointerOnCard = true;
        }

        hoveredSlot = pointerEventData.pointerEnter.GetComponent<BoughtItemUI>();
        Debug.Log("Hovering over: " + hoveredSlot);
    }

    public void DropCard()
    {
        
        if (pointerOnCard)
        {
            Debug.Log("hover off called");
            animator.Play("CardUIOffHover");
            pointerOnCard = false;
        }

        hoveredSlot = null;
    }

    
}