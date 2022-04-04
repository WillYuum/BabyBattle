using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Troops;
using GameplayUI.Helpers;

public enum BuildingType { SmallBulding, LargeBuilding };

public class Building : MonoBehaviour
{
    private float _startingHealth = 100f;
    private float _currentHealth;

    [SerializeField] private HealthBarUI _healthBarUI;

    // private List<Troop> _troops = new List<Troop>();


    void Awake()
    {
        _currentHealth = _startingHealth;
        _healthBarUI.Init();
    }

    public void TakeDamage(BuildingTakeDamageAction damageAction)
    {
        _currentHealth -= damageAction.damageAmountOnBuilding;
        _healthBarUI.SetHealth(_currentHealth / _startingHealth, 0.1f);

        if (_currentHealth <= 0)
        {
            // MakeTroopsLeaveBuilding(damageAction);
            DestroyBuilding();
        }
    }

    public virtual void DestroyBuilding()
    {
        Destroy(gameObject, 2.0f);
    }

    // private void MakeTroopsLeaveBuilding(BuildingTakeDamageAction damageAction)
    // {
    //     if (_troops.Count == 0) return;

    //     var troopTakeDamageAction = new TroopTakeDamageAction
    //     {
    //         DamageAmount = damageAction.damageAmountOnTroop,
    //         DamagedByTroop = damageAction.damageByTroop,
    //     };

    //     foreach (Troop troop in _troops)
    //     {
    //         troop.TakeDamage(troopTakeDamageAction);
    //     }
    // }

    private void OnMouseDown()
    {
        TakeDamage(new BuildingTakeDamageAction
        {
            damageAmountOnBuilding = 10f,
            damageAmountOnTroop = 10f,
            damageByTroop = TroopType.BabyTroop,
        });
    }
}





public struct BuildingTakeDamageAction
{
    public float damageAmountOnBuilding;
    public float damageAmountOnTroop;
    public TroopType damageByTroop;
}