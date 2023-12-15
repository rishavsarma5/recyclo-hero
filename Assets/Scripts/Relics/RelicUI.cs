using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RelicUI : MonoBehaviour
{
    public Relic relic;

    public TMP_Text relicDescription;
    public TMP_Text relicName;
    public Image relicImage;

    //public GameObject discardEffect;
    BattleSceneManager battleSceneManager;
    GameManager gameManager;
    TopBar topBar;

    // Animator animator;
    private void Awake()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        gameManager = FindObjectOfType<GameManager>();
        topBar = FindObjectOfType<TopBar>();
        //animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //animator.Play("HoverOffCard");
    }

    public void LoadRelicUI(Relic _relic)
    {
        relic = _relic;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        relicName.text = relic.relicName;
        relicDescription.text = relic.relicDescription;
        relicImage.sprite = relic.relicImage;
    }

    public void SelectRelicUI()
    {
        relic.AddRelicEffect(battleSceneManager, battleSceneManager.player);
        AddRelicToTopBar();
        TurnOffUI();
    }

    public void DeselectSpecialPowerOption()
    {
        //animator.Play("HoverOffCard");
    }

    public void AddRelicToTopBar()
    {
        topBar.AddRelicItem(relic);
    }

    public void TurnOffUI()
    {
        foreach (RelicUI relicUI in battleSceneManager.relicsDisplayed)
        {
            relicUI.gameObject.SetActive(false);
        }
        battleSceneManager.relicSelectMenu.SetActive(false);
        battleSceneManager.startingRelicChosen = true;
    }
}
