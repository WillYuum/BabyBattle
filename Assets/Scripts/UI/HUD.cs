using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.GenericSingletons;


namespace HUDCore
{
    public class HUD : MonoBehaviourSingleton<HUD>
    {
        public Action OnUpdateTroopsSpawnCount;
        public Action OnUpdateToysCount;

    }
}