using UnityEngine;

public abstract class CardElement : ScriptableObject
{
    public string elementName;
    public string description;
    public Color color;

    public abstract void DoSideEffect();

    public abstract float ResistancePercentage();
}