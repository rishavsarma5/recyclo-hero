using UnityEngine;

public abstract class SpecialPowerOption: ScriptableObject
{
    public string title;
    public string description;
    public Sprite image;

    public abstract void PerformAction();
}
