using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
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
    //[HideInInspector] public Transform cardParentAfterDrag;
    public BoughtItemUI boughtItemSlotParent;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool pointerOnCard = false;
    private Vector2 originalPosition;
    private bool isDragged = false;

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
        Debug.Log("original position:" + originalPosition);
        //cardParentAfterDrag = null;
        cardParentMenu = transform.parent;
    }

    private void OnEnable()
    {
        //animator.Play("CardUIOffHover");
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
        /*
        if (cardParentAfterDrag != null && boughtItemSlotParent)
        {
            Debug.Log("got here");
            transform.SetParent(transform.parent);
            Debug.Log("original position:" + originalPosition);
            this.rectTransform.anchoredPosition = originalPosition;
            cardParentAfterDrag = null;
            boughtItemSlotParent = null;
            card.RemoveFromInventory(battleSceneManager, this);
        }
        */
        /*
        if (boughtItemSlotParent)
        {
            Debug.Log("got here");
            boughtItemSlotParent.cardInSlot = null;
            transform.SetParent(transform.parent);
            Debug.Log("original position:" + originalPosition);
            this.rectTransform.anchoredPosition = originalPosition;
            //cardParentAfterDrag = null;
            boughtItemSlotParent = null;
            card.RemoveFromInventory(battleSceneManager, this);
        }
        */
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (battleSceneManager.coins - card.GetCardTier() < 0)
        {
            // Cancel the potential drag
            eventData.pointerDrag = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        if (battleSceneManager.coins - card.GetCardTier() >= 0)
        {
            isDragged = true;
            //transform.SetParent(transform.root);
            //cardParentAfterDrag = transform.parent;
            transform.SetAsLastSibling();
            canvasGroup.blocksRaycasts = false;
        } 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag");
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag called now");

        if (!boughtItemSlotParent)
        {
            this.rectTransform.anchoredPosition = originalPosition;
            canvasGroup.blocksRaycasts = true;
        } else
        {
            canvasGroup.blocksRaycasts = true;
        }

        //AfterDragEnds();
        isDragged = false;
    }

    public void AfterDragEnds()
    {
        if (isDragged && boughtItemSlotParent)
        {
            Debug.Log("this gets called here");
            card.AddToInventory(battleSceneManager, this);
            Debug.Log("BattleSceneManager bought Items List Count: " + battleSceneManager.itemsBought.Count);
        }
    }

    public void DeselectCard()
    {
        battleSceneManager.selectedCard = null;
        if (boughtItemSlotParent)
        {
            Debug.Log("got here");
            boughtItemSlotParent.cardInSlot = null;
            transform.SetParent(transform.parent);
            Debug.Log("original position:" + originalPosition);
            this.rectTransform.anchoredPosition = originalPosition;
            //cardParentAfterDrag = null;
            boughtItemSlotParent = null;
            card.RemoveFromInventory(battleSceneManager, this);
        }
    }

    public void HoverCard()
    {
        if (!pointerOnCard)
        {
            animator.Play("CardUIOnHover");
            Debug.Log("hover on called");
            pointerOnCard = true;
        }
    }

    public void DropCard()
    {
        
        if (pointerOnCard)
        {
            Debug.Log("hover off called");
            animator.Play("CardUIOffHover");
            pointerOnCard = false;
        }
            
    }
}