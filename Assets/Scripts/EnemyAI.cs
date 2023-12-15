using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyAI
{
    [Serializable]
    public struct PotentialEnemyAction
    {
        [FormerlySerializedAs("EnemyAction")] [Tooltip("Enemy Action to execute")]
        public EnemyAction enemyAction;

        [FormerlySerializedAs("Weight")] [Tooltip("Weight the Enemy Action has")]
        public int weight;
    }
    
    [Tooltip("List of EnemyLogicConditions used to pick an action")] [SerializeField]
    private List<EnemyLogicCondition> conditions = new();

    [Tooltip("List of Possible Enemy Actions that can be performed if the enemy is Enraged")] [SerializeField]
    private List<PotentialEnemyAction> enragedActions = new();

    [Tooltip("Is the enemy enraged?")] [SerializeField]
    private GlobalBool enraged;

    [Tooltip("Default Enemy Action")] [SerializeField]
    private EnemyAction defaultEnemyAction;

    public EnemyAction PickAction()
    {
        if (enraged && enraged.CurrentValue && enragedActions is { Count: > 0 })
        {
            return PickEnemyAction(enragedActions);
        }
        
        EnemyLogicCondition pickedCondition = conditions.FirstOrDefault(condition => condition.Compare());

        return pickedCondition == null ? defaultEnemyAction : PickEnemyAction(pickedCondition.EnemyActionChoices);
    }
    
    public EnemyAction PickEnemyAction(List<PotentialEnemyAction> potentialEnemyActions)
    {
        int totalWeight = potentialEnemyActions.Sum(choice => choice.weight);

        int pickedNum = Random.Range(0, totalWeight);

        int accumulator = 0;
        foreach (PotentialEnemyAction choice in potentialEnemyActions)
        {
            if (pickedNum >= accumulator && pickedNum < accumulator + choice.weight)
            {
                return choice.enemyAction;
            }

            accumulator += choice.weight;
        }

        return null;
    }
}