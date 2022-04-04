using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.GenericSingletons;


public class GameManager : MonoBehaviourSingleton<GameManager>
{
    void Start()
    {
        AudioManager.instance.Load();
    }

}
