using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Cards")] public List<Card> currentCards = new List<Card>();
    public CardUI selectedCard;
    public List<CardUI> cardsDisplayed = new List<CardUI>();
    public List<SpecialPowerOption> playerSpecialPowerCards = new List<SpecialPowerOption>();
    public List<SpecialPowerOption> enemySpecialPowerMoves = new List<SpecialPowerOption>();
    public List<RelicUI> relicsDisplayed = new List<RelicUI>();
    public List<SpecialPowerUI> playerSpecialPowerCardsDisplayed = new List<SpecialPowerUI>();
    public List<BoughtItemUI> boughtItemSlots = new List<BoughtItemUI>();
    //public List<string> activeRelics = new List<string>(); // Change to relic list
    //public List<string> deActiveRelics = new List<string>(); // Change to relic list

    [Header("Stats")] public Enemy enemy;
    public Player player;
    public int coins;
    public int waterSECoinBonus;
    public int relicCoinsBonus;
    public int powerBarIncrement;
    public int playerStatusEffectSPChange;
    public int enemyStatusEffectSPChange;
    public int drawAmount = 5;
    public int relicPickAmount = 3;
    public List<CardUI> itemsBought = new List<CardUI>();
    public int maxWeaponsBought = 2;
    public int relicMaxWeaponsBonus;
    public int maxArmorBought = 1;
    public Turn turn;

    public enum Turn
    {
        Player,
        Enemy
    };

    public Phase phase;

    [Header("UI")] public Button continueButton;
    public TopBar topBar;
    public TMP_Text coinText;
    public Image coinImage;
    public Button rollButton;
    public TMP_Text rollButtonText;
    public Button cardActionButton;
    public Image cardActionBackground;
    public GameObject cardMenu;
    public GameObject specialPowerMenu;
    public GameObject relicSelectMenu;
    public TMP_Text cardActionButtonText;

    public TMP_Text phaseText;
    public Animator bannerAnimator;
    //public Transform topParent;
    //public Transform enemyParent;
    //public EndScreen endScreen;

    [Header("Enemies")] public List<Enemy> enemies = new List<Enemy>();
    public GameObject[] possibleEnemies;


    CardActions cardActions;
    GameManager gameManager;
    SpecialPower spBar;
    public TMP_Text turnText;
    public GameObject gameOver;
    public GameObject youWin;
    public int currDiceSides;
    public bool allItemsBought;
    public bool playerAttackOver;
    public bool coinsRolled;
    public bool specialPowerRolled;
    public bool rollEnemySpecialPower;
    public bool specialPowerOptionChosen;
    public bool resetSpecialPowerLevel;
    public bool startingRelicChosen;
    public int numTurns;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cardActions = GetComponent<CardActions>();
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        spBar = FindObjectOfType<SpecialPower>();
        topBar = FindObjectOfType<TopBar>();
        //endScreen = FindObjectOfType<EndScreen>();
        phase = Phase.PostTurnCleanup;
        numTurns = 1;
        relicCoinsBonus = 0;
        relicMaxWeaponsBonus = 0;
        rollEnemySpecialPower = false;
        specialPowerOptionChosen = false;
        resetSpecialPowerLevel = false;
        startingRelicChosen = false;
    }

    public void StartFight()
    {
        player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        StartCoroutine(ChooseStartingRelic());
    }

    public IEnumerator ChooseStartingRelic()
    {
        relicSelectMenu.SetActive(true);
        gameManager.relics.Shuffle();
        for (int i = 0; i < relicPickAmount; i++)
        {
            RelicUI relicUI = relicsDisplayed[i];
            relicUI.LoadRelicUI(gameManager.relics[i]);
            relicUI.gameObject.SetActive(true);
        }

        yield return new WaitUntil(() => startingRelicChosen);
        startingRelicChosen = false;
        player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        ContinueToNextPhase();
    }

    public IEnumerator BuyPhase()
    {
        if (enemy.currentEnemy == null)
        {
            enemy.SetupEnemy();
        }

        turnText.text = "Player's Turn";
        bannerAnimator.Play("BannerOut");

        allItemsBought = false;
        maxArmorBought = 1;
        maxWeaponsBought = 2 + relicMaxWeaponsBonus;
        itemsBought.Clear();
        coins = relicCoinsBonus;
        coinsRolled = false;

        rollButton.gameObject.SetActive(true);
        rollButtonText.gameObject.SetActive(true);
        rollButtonText.text = "Roll d6 for coins this round:";
        currDiceSides = 6;
        continueButton.gameObject.SetActive(false);
        yield return new WaitUntil(() => coinsRolled);
        if (player.isCoinBoosted)
        {
            coins += waterSECoinBonus;
        }

        foreach (CardUI cardUI in cardsDisplayed)
        {
            cardUI.gameObject.SetActive(false);
        }

        currentCards.Clear();

        rollButton.gameObject.SetActive(false);
        rollButtonText.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);

        gameManager.inventory.Shuffle();
        cardMenu.gameObject.SetActive(true);
        coinImage.gameObject.SetActive(true);
        coinText.gameObject.SetActive(true);
        coinText.text = coins.ToString();
        DrawCards(drawAmount);
        DisplayBoughtItemSlots();

        enemy.GenerateAttack();

        yield return new WaitUntil(() => allItemsBought);
        coinImage.gameObject.SetActive(false);
        coinText.gameObject.SetActive(false);
        
        cardMenu.gameObject.SetActive(false);
        foreach (CardUI cardUI in cardsDisplayed)
        {
            cardUI.gameObject.SetActive(false);
        }
        foreach (BoughtItemUI slotUI in boughtItemSlots)
        {
            slotUI.gameObject.SetActive(false);
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
        CardUI cardUI = cardsDisplayed[currentCards.Count - 1];
        cardUI.LoadCard(card);
        cardUI.gameObject.SetActive(true);
    }

    public void DisplayBoughtItemSlots()
    {
        foreach (BoughtItemUI slotUI in boughtItemSlots)
        {
            slotUI.gameObject.SetActive(true);
        }
    }

    public void ContinueToNextPhase()
    {
        if (phase == Phase.ApplyRelics)
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
            phase = Phase.PostTurnCleanup;
            StartCoroutine(PostTurnCleanupPhase());
        }
        else if (phase == Phase.PostTurnCleanup)
        {
            //phase = Phase.ApplyRelics;
            //StartCoroutine(ApplyRelics()); // use this and above line instead
            phase = Phase.Buy;
            phaseText.text = "BUY PHASE";
            StartCoroutine(BuyPhase());
        }
    }

    public IEnumerator AttackPhase()
    {
        continueButton.gameObject.SetActive(true);
        

        if (!player.isStunned)
        {
            if (itemsBought.Count == 0)
            {
                rollButtonText.gameObject.SetActive(true);
                rollButtonText.text = "No weapons bought. Press Continue to go to enemy attack.";
            }
            else
            {
                foreach (CardUI item in itemsBought)
                {
                    item.gameObject.SetActive(true);
                    StartCoroutine(cardActions.PerformAction(item.card, enemy));
                    yield return new WaitUntil(() => cardActions.cardActionOver);
                    item.gameObject.SetActive(false);
                }

                cardActionButton.gameObject.SetActive(false);
                cardActionButtonText.gameObject.SetActive(false);
                cardActionBackground.gameObject.SetActive(false);
                yield return new WaitUntil(() => playerAttackOver);
                playerAttackOver = false;
            }
        }
        else
        {
            Debug.Log("Player is stunned. Skipping Player's Attack Phase.");
        }

        continueButton.gameObject.SetActive(false);
        StartCoroutine(EnemyAttack());
    }

    private IEnumerator EnemyAttack()
    {
        turnText.text = "Enemy's Turn";
        bannerAnimator.Play("BannerIn");

        yield return new WaitForSeconds(1.5f);
        /*
        foreach (Enemy enemy in enemies)
        {
            enemy.enemyTurnOver = false;
            enemy.TakeTurn();
            yield return new WaitUntil(() => enemy.enemyTurnOver);
        }
        */
        Debug.Log($"enemy stun/stagger status: {enemy.isStunned || enemy.staggered.CurrentValue}");
        if (enemy.isStunned)
        {
            enemy.enraged.CurrentValue = false;
            enemy.enragedPanel.SetActive(false);
        }

        if (!enemy.isStunned && !enemy.staggered.CurrentValue)
        {
            enemy.enemyTurnOver = false;
            StartCoroutine(enemy.AttackPlayer());
            yield return new WaitUntil(() => enemy.enemyTurnOver);
            rollButtonText.gameObject.SetActive(false);
            enemy.enemyTurnOver = false;
        }
        else
        {
            Debug.Log("Enemy is stunned. Skipping Enemy's Attack Phase.");
        }

        enemy.ReduceEnemyStaggered();
        enemy.LightShieldHeal();
        enemy.RemoveAttack();

        yield return new WaitForSeconds(1.5f);
        ContinueToNextPhase();
    }

    public IEnumerator SpecialPowerPhase()
    {
        rollButton.gameObject.SetActive(true);
        rollButtonText.gameObject.SetActive(true);
        rollButtonText.text = "Roll d6 to increase special power bar:";
        currDiceSides = 6;

        continueButton.gameObject.SetActive(false);
        yield return new WaitUntil(() => specialPowerRolled);
        rollButton.gameObject.SetActive(false);
        rollButtonText.gameObject.SetActive(false);
        specialPowerRolled = false;
        specialPowerOptionChosen = false;

        StartCoroutine(spBar.IncreasePlayerPowerLevel(powerBarIncrement, playerStatusEffectSPChange));

        continueButton.gameObject.SetActive(true);
        yield return new WaitUntil(() => rollEnemySpecialPower);
        rollEnemySpecialPower = false;
        currDiceSides = enemy.currentHeavyArmor.CurrentValue <= 0
            ? enemy.noHeavyArmorSpecialBarIncrease
            : enemy.standardSpecialBarIncrease;

        int value = Dice.DiceRoll(currDiceSides);
        Debug.Log($"Enemy increased power bar by {value}");
        StartCoroutine(spBar.IncreaseEnemyPowerLevel(value, enemyStatusEffectSPChange));
        yield return new WaitForSeconds(0.5f);
        ContinueToNextPhase();
    }

    public IEnumerator DisplayPlayerSpecialAttackCards()
    {
        specialPowerMenu.SetActive(true);
        for (int i = 0; i < playerSpecialPowerCards.Count; i++)
        {
            SpecialPowerUI specialPowerUI = playerSpecialPowerCardsDisplayed[i];
            specialPowerUI.LoadSpecialPowerUI(playerSpecialPowerCards[i]);
            specialPowerUI.gameObject.SetActive(true);
        }

        yield return new WaitUntil(() => specialPowerOptionChosen);
        resetSpecialPowerLevel = true;
        specialPowerOptionChosen = false;
    }


    public IEnumerator PerformEnemySpecialPower()
    {
        // shuffle the list of enemy's special powers and choose first
        SpecialPowerOption enemySpecial =
            enemy.currentEnemy.enemySpecialActions[Random.Range(0, enemy.currentEnemy.enemySpecialActions.Count)];
        enemySpecial.PerformAction();
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PostTurnCleanupPhase()
    {
        player.UpdateCurrentTempShieldTurns();
        player.RegeneratePermShield();

        for (int i = player.activeStatusEffects.Count - 1; i >= 0; i--)
        {
            player.activeStatusEffects[i].DecreaseTurn();
        }

        for (int i = enemy.activeStatusEffects.Count - 1; i >= 0; i--)
        {
            enemy.activeStatusEffects[i].DecreaseTurn();
        }

        yield return new WaitForSeconds(0.5f);
        ContinueToNextPhase();
    }

    public IEnumerator ApplyRelics()
    {
        // apply and update relics
        /*
        foreach (Relic relic in activeRelics)
        {
            relic.ApplyEffect();
            relic.Update();
        }
        */
        yield return new WaitForSeconds(0.5f);
        ContinueToNextPhase();
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
            coins += value;
            Debug.Log($"Rolled for {value} coins.");
            coinsRolled = true;
        }
        else if (phase == Phase.SpecialPower)
        {
            powerBarIncrement = value;
            Debug.Log($"Rolled {value} to add to power bar.");
            specialPowerRolled = true;
        }
    }

    public void EndFight(bool win)
    {
        if (!win)
            gameOver.SetActive(true);
        else
        {
            youWin.SetActive(true);
        }
        enemy.audioSource.Stop();
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

public enum Phase
{
    ApplyRelics,
    Buy,
    SpecialPower,
    Attack,
    PostTurnCleanup,
};