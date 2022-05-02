using System;
using System.Collections;
using System.Collections.Generic;
using GameplayUI.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings.DefensiveWallComponent
{
    public class DefensiveWallUI : MonoBehaviour
    {
        [SerializeField] private HealthBarUI _healthBarUI;
        [SerializeField] private Button _repairButton;
        [SerializeField] private Button _moveForwardButton;

        public struct InitConfig
        {
            public float StartingHealthRatio;
            public Action OnClickRepairButton;
            public Action<EntityDirection> OnClickOnAttackButton;
        }


        public void Init(InitConfig initConfig)
        {
            _healthBarUI.Init();
            _healthBarUI.SetHealth(initConfig.StartingHealthRatio, 0.1f);

            _repairButton.onClick.AddListener(() => initConfig.OnClickRepairButton());

            //NOTE: Implemented this in fast way possible, I get direction depending on  the position of button relative of the center of the UI
            //NOTE: So if button is on the left of the center, it will return EntityDirection.Left
            EntityDirection direction = transform.position.x - _moveForwardButton.transform.position.x > 0 ? EntityDirection.Right : EntityDirection.Left;
            _moveForwardButton.onClick.AddListener(() => initConfig.OnClickOnAttackButton(direction));
        }

        public void UpdateHealthBar(float healthRatio)
        {
            _healthBarUI.SetHealth(healthRatio, 0.1f);
        }
    }
}