using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameplayUI.Helpers;
using InteractableUI;
using UnityEngine;
using UnityEngine.UI;


public class MainCampUI : MonoBehaviour
{
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private TroopCard[] _troopCardPrefab;


    public struct InitConfig
    {
        public bool active;
        public float startingHealth;
        public Action<Troops.TroopType> onClickTroopCard;
    }

    public void ToggleUI(bool active)
    {
        gameObject.SetActive(active);
    }

    public void InitUI(InitConfig config)
    {
        _healthBarUI.Init();
        _healthBarUI.SetHealth(config.startingHealth);

        ToggleUI(config.active);

        AddClickEventToTroopCard(config.onClickTroopCard);
    }

    public void UpdateHealthBar(float healthRatio)
    {
        _healthBarUI.SetHealth(healthRatio);
    }


    private void AddClickEventToTroopCard(Action<Troops.TroopType> action)
    {
        _troopCardPrefab.ToList().ForEach(x => x.GetComponent<Button>().onClick.AddListener(() => action(x.TroopType)));
    }
}
