using UnityEngine;
using System;
using UnityEngine.UI;
using GameplayUI.Helpers;

namespace Buildings.TroopCampComponent
{
    public class TroopCampUI : MonoBehaviour
    {
        [SerializeField] private Button _repairButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private HealthBarUI _healthBarUI;

        public struct InitConfig
        {
            public float StartingHealthRatio;
            public Action OnClickRepairButton;
            public Action<EntityDirection> OnClickOnArrowButton;
        }


        public void Init(InitConfig initConfig)
        {
            _healthBarUI.Init();
            _healthBarUI.SetHealth(initConfig.StartingHealthRatio, 0.1f);

            _repairButton.onClick.AddListener(() => initConfig.OnClickRepairButton());
            _leftButton.onClick.AddListener(() => initConfig.OnClickOnArrowButton(EntityDirection.Left));
            _rightButton.onClick.AddListener(() => initConfig.OnClickOnArrowButton(EntityDirection.Right));
        }


        public void UpdateHealthBar(float healthRatio)
        {
            _healthBarUI.SetHealth(healthRatio, 0.1f);
        }
    }
}