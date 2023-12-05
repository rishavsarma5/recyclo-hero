using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAction
{
    public AttackType intentType;
    public enum AttackType { NormalAttack, SpecialAttack }
    public int diceSides;
    public int numAttacks;
    public int baseDamage;
}
