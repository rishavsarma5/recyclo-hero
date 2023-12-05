using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Cards")] public List<Card> currentCards = new List<Card>();
    public CardUI selectedCard;
    public List<CardUI> cardsDisplayed = new List<CardUI>();
    public List<SpecialPowerOption> specialPowerCards = new List<SpecialPowerOption>();

    [Header("Stats")] public Enemy enemy;
    public Player player;
    public int coins;
    public int powerBarIncrement;
    public int drawAmount = 5;
    public List<CardUI> itemsBought = new List<CardUI>();
    public int maxWeaponsBought = 2;
    public int maxArmorBought = 1;
    public Turn turn;

    public enum Turn
    {
        Player,
        Enemy
    };

    public Phase phase;

    public enum Phase
    {
        Prep,
        Buy,
        SpecialPower,
        Attack
    };

    [Header("UI")] public Button continueButton;
    public TMP_Text coinText;
    public Image coinImage;
    public Button rollButton;
    public TMP_Text rollButtonText;
    public Button cardActionButton;

    public TMP_Text cardActionButtonText;
    public TMP_Text phaseText;
    //public Transform topParent;
    //public Transform enemyParent;
    //public EndScreen endScreen;

    [Header("Enemies")] public List<Enemy> enemies = new List<Enemy>();
    public GameObject[] possibleEnemies;


    CardActions cardActions;
    GameManager gameManager;
    SpecialPower spBar;
    public Animator banner;
    public TMP_Text turnText;
    public Image turnImage;
    public GameObject gameover;
    public int currDiceSides;
    public bool allItemsBought;
    public bool playerAttackOver;
    public bool coinsRolled;
    public bool specialPowerRolled;
    public bool rollEnemySpecialPower;
    public int numTurns;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cardActions = GetComponent<CardActions>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        spBar = FindObjectOfType<SpecialPower>();
        //endScreen = FindObjectOfType<EndScreen>();
        phase = Phase.Prep;
        numTurns = 1;
        rollEnemySpecialPower = false;
    }

    public void StartFight()
    {
        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        ContinueToNextPhase();
    }

    public IEnumerator BuyPhase()
    {
        phase = Phase.Buy;
        phaseText.text = "BUY PHASE";
        allItemsBought = false;
        maxArmorBought = 1;
        maxWeaponsBought = 2;
        itemsBought.Clear();
        coins = 0;
        coinsRolled = false;
        coinImage.gameObject.SetActive(true);
        coinText.gameObject.SetActive(true);
        coinText.text = coins.ToString();

        rollButton.gameObject.SetActive(true);
        rollButtonText.gameObject.SetActive(true);
        rollButtonText.text = "Roll d6 for coins this round:";
        currDiceSides = 6;
        continueButton.gameObject.SetActive(false);
        yield return new WaitUntil(() => coinsRolled);
        coinText.text = coins.ToString();

        foreach (CardUI cardUI in cardsDisplayed)
        {
            cardUI.gameObject.SetActive(false);
        }

        currentCards.Clear();

        rollButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
        rollButtonText.text = "Choose items you want to use for the round:";
        //rollButtonText.gameObject.SetActive(false);
        //turnText.text = "Player's Turn";
        //banner.Play("bannerOut");

        //playerIcon.SetActive(true);

        //GameObject newEnemy = Instantiate(prefabsArray[Random.Range(0, prefabsArray.Length)], enemyParent);
        //endScreen = FindObjectOfType<EndScreen>();
        //if (endScreen != null)
        //    endScreen.gameObject.SetActive(false);

        //Enemy[] eArr = FindObjectsOfType<Enemy>();
        //enemies = new List<Enemy>();


        gameManager.inventory.Shuffle();
        DrawCards(drawAmount);
        yield return new WaitUntil(() => allItemsBought);
        rollButtonText.gameObject.SetActive(false);
        coinImage.gameObject.SetActive(false);
        coinText.gameObject.SetActive(false);

        foreach(CardUI cardUI in cardsDisplayed)
        {
            cardUI.gameObject.SetActive(false);
        }
        ContinueToNextPhase();
    }

    public void DrawCards(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            currentCards.Add(gameManager.inventory[i]);
            DisplayCardInHand(currentCards[i]);
        }
    }

    public void DisplayCardInHand(Card card)
    {
        Debug.Log($"cardsDisplayedCount {cardsDisplayed.Count}, currentCardsCount {currentCards.Count}", this);
        CardUI cardUI = cardsDisplayed[currentCards.Count - 1];
        cardUI.LoadCard(card);
        cardUI.gameObject.SetActive(true);
    }

    public void ContinueToNextPhase()
    {
        if (phase == Phase.Prep)
        {
            phase = Phase.Buy;
            phaseText.text = "BUY PHASE";
            StartCoroutine(BuyPhase());
        }
        else if (phase == Phase.Buy)
        {
            phase = Phase.SpecialPower;
            phaseText.text = "SPECIAL POWER PHASE";
            StartCoroutine(SpecialPowerPhase());
        }
        else if (phase == Phase.SpecialPower)
        {
            phase = Phase.Attack;
            phaseText.text = "ATTACK PHASE";
            StartCoroutine(AttackPhase());
        }
        else if (phase == Phase.Attack)
        {
            phase = Phase.Buy;
            phaseText.text = "BUY PHASE";
            player.UpdateCurrentTempShieldTurns();
            turnText.gameObject.SetActive(false);
            turnImage.gameObject.SetActive(false);
            StartCoroutine(BuyPhase());
        }
    }

    public IEnumerator AttackPhase()
    {
        turnText.gameObject.SetActive(true);
        turnImage.gameObject.SetActive(true);
        turnText.text = "Player's Turn";
        continueButton.gameObject.SetActive(true);
        //banner.Play("bannerIn");

        if (itemsBought.Count == 0)
        {
            rollButtonText.gameObject.SetActive(true);
            rollButtonText.text = "No weapons bought. Press Continue to go to enemy attack.";
        } else
        {
            foreach (CardUI item in itemsBought)
            {
                Debug.Log($"Using item {item}...");
                item.gameObject.SetActive(true);
                StartCoroutine(cardActions.PerformAction(item.card, enemy));
                //item.gameObject.SetActive(false);
                yield return new WaitUntil(() => cardActions.cardActionOver);
                item.gameObject.SetActive(false);
            }
            cardActionButton.gameObject.SetActive(false);
            cardActionButtonText.gameObject.SetActive(false);
            yield return new WaitUntil(() => playerAttackOver);
            playerAttackOver = false;
        }
        continueButton.gameObject.SetActive(false);
        StartCoroutine(EnemyAttack());
    }
    private IEnumerator EnemyAttack()
    {
        turnText.text = "Enemy's Turn";
        //banner.Play("bannerIn");

        // yield return new WaitForSeconds(1.5f);
        /*
        foreach (Enemy enemy in enemies)
        {
            enemy.enemyTurnOver = false;
            enemy.TakeTurn();
            yield return new WaitUntil(() => enemy.enemyTurnOver);
        }
        */
        enemy.enemyTurnOver = false;
        StartCoroutine(enemy.AttackPlayer());
        yield return new WaitUntil(() => enemy.enemyTurnOver);
        rollButtonText.gameObject.SetActive(false);
        enemy.enemyTurnOver = false;
        yield return new WaitForSeconds(1.5f);
        ContinueToNextPhase();
    }

    public IEnumerator SpecialPowerPhase()
    {
        turnText.text = "Player's Turn";
        rollButton.gameObject.SetActive(true);
        rollButtonText.gameObject.SetActive(true);
        rollButtonText.text = "Roll d6 to increase special power bar this round:";
        currDiceSides = 6;

        continueButton.gameObject.SetActive(false);
        yield return new WaitUntil(() => specialPowerRolled);
        rollButton.gameObject.SetActive(false);
        rollButtonText.gameObject.SetActive(false);
        specialPowerRolled = false;
        spBar.IncreasePlayerPowerLevel(powerBarIncrement);

        continueButton.gameObject.SetActive(true);
        yield return new WaitUntil(() => rollEnemySpecialPower);

        turnText.text = "Enemy's Turn";
        currDiceSides = 6;

        int value = Dice.DiceRoll(currDiceSides);
        Debug.Log($"Enemy rolled a {value} to add to power bar");
        spBar.IncreaseEnemyPowerLevel(value);
        yield return new WaitForSeconds(1.5f);
        ContinueToNextPhase();

    }

    public void DisplayPlayerSpecialAttackCards()
    {

    }

    public void PerformEnemySpecialAttack()
    {

    }

    public void ContinueButton()
    {
        if (phase == Phase.Buy)
        {
            allItemsBought = true;
        }
        else if (phase == Phase.Attack)
        {
            playerAttackOver = true;
        }
        else if (phase == Phase.SpecialPower)
        {
            rollEnemySpecialPower = true;
        }
    }

    public void RollButton()
    {
        int value = Dice.DiceRoll(currDiceSides);
        if (phase == Phase.Buy)
        {
            coins = value;
            Debug.Log($"Rolled for {value} coins.");
            coinsRolled = true;
        } else if (phase == Phase.SpecialPower)
        {
            powerBarIncrement = value;
            Debug.Log($"Rolled {value} to add to power bar.");
            specialPowerRolled = true;
        }
    }

    public void EndFight(bool win)
    {
        if (!win)
            gameover.SetActive(true);
    }

    /*
    public void HandleEndScreen()
    {
        //gold
        endScreen.gameObject.SetActive(true);
        endScreen.goldReward.gameObject.SetActive(true);
        endScreen.cardRewardButton.gameObject.SetActive(true);

        endScreen.goldReward.relicName.text = enemies[0].goldDrop.ToString() + " Gold";
        gameManager.UpdateGoldNumber(gameManager.goldAmount += enemies[0].goldDrop);

        //relics
        if (enemies[0].nob)
        {
            gameManager.relicLibrary.Shuffle();
            endScreen.relicReward.gameObject.SetActive(true);
            endScreen.relicReward.DisplayRelic(gameManager.relicLibrary[0]);
            gameManager.relics.Add(gameManager.relicLibrary[0]);
            gameManager.relicLibrary.Remove(gameManager.relicLibrary[0]);
            playerStatsUI.DisplayRelics();
        }
        else
        {
            endScreen.relicReward.gameObject.SetActive(false);
        }

    }
    */
}