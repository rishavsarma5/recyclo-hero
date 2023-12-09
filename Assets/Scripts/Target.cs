using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public bool isCoinBoosted = false;
    public bool isSPBoosted = false;
    public bool isStunned = false;
    public bool isSPNerfed = false;
}
