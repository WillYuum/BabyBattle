using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;
using Utils.GenericSingletons;
using Buildings;

public class GameVariables : MonoBehaviourSingletonPersistent<GameVariables>
{
    [SerializeField] public Variable<TroopType, TroopVariable> TroopVariables;
    [field: SerializeField] public Variable<BuildingType, BuildingVariable> BuildingVariables { get; private set; }
}

[System.Serializable]
public class Variable<T, V>
where T : struct
where V : struct
{
    [SerializeField] public VariableWithType<T, V>[] data;
    public V GetVariable(T type)
    {
        foreach (var variable in data)
        {
            if (variable.Type.Equals(type))
            {
                return variable.Variable;
            }
        }

#if UNITY_EDITOR
        Debug.LogError("Variable with type " + type + " not found");
#endif

        return default(V);
    }
}


[System.Serializable]
public struct VariableWithType<T, V>
{
    [SerializeField] public T Type;
    [SerializeField] public V Variable;
}


[System.Serializable]
public struct TroopVariable
{
    [SerializeField] public float MoveSpeed;
    [SerializeField] public float StartingHealth;
    [SerializeField] public float Cost;
    [SerializeField] public float AttackDelay;
    [SerializeField] public float Damage;
}



[System.Serializable]
public struct BuildingVariable
{
    [SerializeField] public float StartingHealth;
    [SerializeField] public float Cost;
}