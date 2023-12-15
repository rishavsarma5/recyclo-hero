using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyLogicCondition
{
    private enum Comparator
    {
        GREATER_THAN,
        GREATER_THAN_EQUAL,
        EQUAL,
        LESS_THAN_EQUAL,
        LESS_THAN,
        NOT_EQUAL
    };

    [FormerlySerializedAs("variable")]
    [Tooltip("The current value you are checking against the max value.")]
    [SerializeField]
    private GlobalInt currentValue;

    [Tooltip("The max value you are checking against the current value.")] [SerializeField]
    private GlobalInt maxValue;

    [Tooltip("Is currentValue [comparator] comparisonPercentage of maxValue")] [SerializeField]
    private Comparator comparator = Comparator.EQUAL;

    [FormerlySerializedAs("comparisonNumber")] [Tooltip("Comparison percentage.")] [SerializeField] [Range(0.0f, 1.0f)]
    private float comparisonPercentage;

    [Tooltip("If Comparison is TRUE then do this EnemyAction if it is picked")]
    public List<EnemyAI.PotentialEnemyAction> EnemyActionChoices;

    public bool Compare()
    {
        if (!currentValue || !maxValue) return false;

        float checkedValue = currentValue.CurrentValue / (float)maxValue.CurrentValue;
        
        Debug.Log($"Compare checkedValue = {checkedValue} and comparator = {comparator}");

        return comparator switch
        {
            Comparator.GREATER_THAN => checkedValue > comparisonPercentage,
            Comparator.GREATER_THAN_EQUAL => checkedValue >= comparisonPercentage,
            Comparator.EQUAL => Math.Abs(checkedValue - comparisonPercentage) < 0.0001f,
            Comparator.LESS_THAN_EQUAL => checkedValue <= comparisonPercentage,
            Comparator.LESS_THAN => checkedValue < comparisonPercentage,
            Comparator.NOT_EQUAL => Math.Abs(checkedValue - comparisonPercentage) > 0.0001f,
            _ => false
        };
    }
}