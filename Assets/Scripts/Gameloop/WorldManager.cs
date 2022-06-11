using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;

public class WorldManager : MonoBehaviourSingleton<WorldManager>
{
    [field: SerializeField] public WorldBorder WorldBorders { get; private set; }
}


[System.Serializable]
public class WorldBorder
{
    [field: SerializeField] public Transform MaxLeft { get; private set; }
    [field: SerializeField] public Transform MaxRight { get; private set; }
}