using UnityEngine;

[CreateAssetMenu]
public class EnemyAction : ScriptableObject
{
    public string actionName;
    public string actionDescription;
    public CardElement actionElement;

    public int diceSides;
    public int numAttacks;
    public int baseDamage;
}