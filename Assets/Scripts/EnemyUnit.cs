using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class EnemyUnit : ScriptableObject
{
    [Header("Description")] [Tooltip("Name")]
    public string enemyName;

    public string enemyDescription;

    [Header("Visuals")] public Sprite enemyImage;

    [Header("Audio")] public AudioClip bossMusic;

    [Header("HP Stats")] [Tooltip("Health Points")]
    public int health;

    [FormerlySerializedAs("lightArmor")] [Tooltip("Light Armor health points")]
    public int lightShield;

    [Tooltip("Heavy Armor Health Points")] public int heavyArmor;

    [Header("Misc. Stats")] [Tooltip("Enraged Damage Multiplier")]
    public float enragedMultiplier = 1.0f;

    [Tooltip("Staggered Turn Count")] public int staggeredTurns;

    [Header("Attacks")] [Tooltip("List of Enemy Actions/Attacks")]
    public List<EnemyAction> enemyActions = new List<EnemyAction>();

    [Tooltip("List of Special Actions")] public List<SpecialPowerOption> enemySpecialActions;

    [Tooltip("Enemy AI")] [SerializeField] private EnemyAI enemyAI;

    public EnemyAction NextEnemyAction()
    {
        return enemyAI == null ? enemyActions[Random.Range(0, enemyActions.Count)] : enemyAI.PickAction();
    }
}