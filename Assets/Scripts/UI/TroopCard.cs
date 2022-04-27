using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InteractableUI
{
    public class TroopCard : MonoBehaviour
    {
        [SerializeField] private Troops.TroopType _troopType;

        public Troops.TroopType TroopType
        {
            get { return _troopType; }
        }
    }
}