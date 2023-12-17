using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Card card;
    public TMP_Text cardTitleText;
    public TMP_Text cardDescriptionText;
    public TMP_Text cardTierText;
    public TMP_Text cardElementText;
    public TMP_Text cardTypeText;

    public Image cardImage;
    public Image cardTierImage;
    public Image cardUIParentImage;
    [HideInInspector] public Transform cardParentAfterDrag;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    //public GameObject discardEffect;
    BattleSceneManager battleSceneManager;

    // Animator animator;
    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        //animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //animator.Play("HoverOffCard");
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
        if (battleSceneManager.coins - card.GetCardTier() >= 0)
        {
            card.AddToInventory(battleSceneManager, this);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        cardParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        //cardUIParentImage.raycastTarget = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag");
        //transform.position = Input.mousePosition;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(cardParentAfterDrag);
        //cardUIParentImage.raycastTarget = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeselectCard()
    {
        //Debug.Log("card is deselected");
        battleSceneManager.selectedCard = null;
        //animator.Play("HoverOffCard");
    }

    public void HoverCard()
    {
        //if (battleSceneManager.selectedCard == null)
        //animator.Play("HoverOnCard");
    }

    public void DropCard()
    {
        //if (battleSceneManager.selectedCard == null)
        //animator.Play("HoverOffCard");
    }

    public void HandleDrag()
    {
    }

    public void HandleEndDrag()
    {
        /*
        if (battleSceneManager.energy < card.GetCardCostAmount())
            return;

        if (card.cardType == Card.CardType.Attack)
        {
            battleSceneManager.PlayCard(this);
            animator.Play("HoverOffCard");
        }
        else if (card.cardType != Card.CardType.Attack)
        {
            animator.Play("HoverOffCard");
            battleSceneManager.PlayCard(this);
        }
        */
    }
}