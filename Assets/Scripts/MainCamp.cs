using System.Collections;
using System.Collections.Generic;
using GameplayUI.Helpers;
using UnityEngine;
using SpawnManagerCore;
using InteractableUI;
using System.Linq;
using UnityEngine.UI;

public class MainCamp : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _currentHealth = 100f;
    private EntityDirection _spawnDirection = EntityDirection.Left;

    [Header("References")]
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private TroopCard[] _troopCardPrefab;

    private void Awake()
    {
        _healthBarUI.Init();
        _healthBarUI.SetHealth(_currentHealth / 100f);

        _troopCardPrefab.ToList().ForEach(x => x.GetComponent<Button>().onClick.AddListener(() => OnClickTroopCard(x.TroopType)));
    }


    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBarUI.SetHealth(_currentHealth / 100f);

        if (_currentHealth <= 0)
        {
            Debug.Log("Building destroyed");
            GameloopManager.instance.MainBuildingDestroyed();
        }
    }

    private void OnClickTroopCard(Troops.TroopType troopType)
    {
        print("Spawning troop: " + troopType);

        var troop = SpawnManager.instance.SpawnFriendlyTroop(troopType, transform.position);
        troop.InitTroop(_spawnDirection);
    }

}
