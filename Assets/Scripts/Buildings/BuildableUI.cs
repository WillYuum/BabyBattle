using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings.BuildableComponent
{
    public class BuildableUI : MonoBehaviour
    {

        [SerializeField] private Button _troopCampButton;
        [SerializeField] private Button _defensiveWallButton;
        public struct InitConfig
        {
            public Action<BuildingType> OnClickBuildingCard;
        }


        public void Init(InitConfig config)
        {
            _troopCampButton.onClick.AddListener(() => config.OnClickBuildingCard(BuildingType.TroopCamp));
            _defensiveWallButton.onClick.AddListener(() => config.OnClickBuildingCard(BuildingType.DefensiveWall));
        }


    }
}
