using UnityEngine;

public abstract class CardElement : ScriptableObject
{
    public string elementName;
    public string description;
    public Color color;
    public float relicBonus;
    public float successMax;
    public Player player;
    public Enemy enemy;
    public BattleSceneManager battleSceneManager;

    public void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        Debug.Log("awake called in card element SO");
    }

    public void SetUnknownValues()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
    }

    public abstract Target DoSideEffect();

    public abstract float ResistancePercentage();
}